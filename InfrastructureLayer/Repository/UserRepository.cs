using RentalSystem.Helpers;
using RentalSystem.Interfaces;
using RentalSystem.Models;
using RentalSystem.Services;
using System.Data.SqlClient;
using System.Data;
using DomainLayer.Models;
using System.Diagnostics;

namespace RentalSystem.Repository
{
    public class UserRepository : IUser
    {
        private readonly QueryBuilder _queryBuilder;

        public UserRepository(QueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }
        public async Task<(IEnumerable<User> Users, int TotalCount)> GetAllUsersWithProfileAndRoleAsync(FilterModel filterModel)
        {
            List<User> users = new List<User>();
            int totalUsers = 0;

            string query = @"
            WITH FilteredUsers AS (
                SELECT u.[Id], u.[Email], u.[FullName], u.[RoleId], 
                       p.[Id] AS ProfileId, p.[PhoneNumber], p.[Passport], 
                       p.[DrivingExperience], p.[Description], p.[ProfileImage],
                       r.[Id] AS UserRoleId, r.[Name] AS RoleName
                FROM [Users] u
                LEFT JOIN [UserProfiles] p ON u.[Id] = p.[UserId]
                LEFT JOIN [Roles] r ON u.[RoleId] = r.[Id]
                WHERE (@Filter IS NULL OR u.FullName LIKE '%' + @Filter + '%')
                AND (@Status IS NULL OR r.Name = @Status)
            )
            SELECT *, COUNT(*) OVER() AS TotalCount
            FROM FilteredUsers
            ORDER BY FullName
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Filter", SqlDbType.NVarChar) { Value = (object)filterModel.Filter ?? DBNull.Value },
                new SqlParameter("@Status", SqlDbType.NVarChar) { Value = (object)filterModel.Status ?? DBNull.Value },
                new SqlParameter("@PageSize", SqlDbType.Int) { Value = filterModel.PageSize },
                new SqlParameter("@Offset", SqlDbType.Int) { Value = (filterModel.Page - 1) * filterModel.PageSize }
            };

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        FullName = reader.GetString(2),
                        RoleId = reader.GetInt32(3),
                        Profile = reader.IsDBNull(4) ? null : new UserProfile
                        {
                            Id = reader.GetInt32(4),
                            PhoneNumber = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Passport = reader.IsDBNull(6) ? null : reader.GetString(6),
                            DrivingExperience = reader.GetInt32(7),
                            Description = reader.IsDBNull(8) ? null : reader.GetString(8),
                            ProfileImage = reader.IsDBNull(9) ? null : reader.GetString(9),
                            UserId = reader.GetInt32(0)
                        },
                        Role = reader.IsDBNull(10) ? null : new Role
                        {
                            Id = reader.GetInt32(10),
                            Name = reader.GetString(11)
                        }
                    });
                    totalUsers = reader.GetInt32(reader.FieldCount - 1);
                }
            }, parameters);

            return (users, totalUsers);
        }
        public async Task<bool> AddUserAsync(User user)
        {
            string salt = SecurityHelper.GenerateSalt(70);
            string hashedPassword = SecurityHelper.HashPassword(user.HashPassword, salt, 10101, 70);

            string query = @"
            INSERT INTO [Users] ([Email], [FullName], [RoleId], [HashPassword], [Salt])
            OUTPUT INSERTED.Id
            VALUES (@Email, @FullName, @RoleId, @HashPassword, @Salt)";

            SqlParameter[] parameters = {
                new SqlParameter("@Email", SqlDbType.NVarChar) { Value = user.Email },
                new SqlParameter("@FullName", SqlDbType.NVarChar) { Value = user.FullName },
                new SqlParameter("@RoleId", SqlDbType.Int) { Value = user.RoleId },
                new SqlParameter("@HashPassword", SqlDbType.NVarChar) { Value = hashedPassword },
                new SqlParameter("@Salt", SqlDbType.NVarChar) { Value = salt }
            };

            int userId = await _queryBuilder.ExecuteScalarAsync<int>(query, parameters);

            if (userId > 0)
            {
                
                if (user.Profile != null)
                {
                    string profileQuery = @"
                    INSERT INTO [UserProfiles] ([UserId], [PhoneNumber], [Passport], [DrivingExperience], [Description], [ProfileImage])
                    VALUES (@UserId, @PhoneNumber, @Passport, @DrivingExperience, @Description, @ProfileImage)";

                    SqlParameter[] profileParameters = {
                        new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                        new SqlParameter("@PhoneNumber", SqlDbType.NVarChar) { Value = (object?)user.Profile.PhoneNumber ?? DBNull.Value },
                        new SqlParameter("@Passport", SqlDbType.NVarChar) { Value = (object?)user.Profile.Passport ?? DBNull.Value },
                        new SqlParameter("@DrivingExperience", SqlDbType.Int) { Value = user.Profile.DrivingExperience },
                        new SqlParameter("@Description", SqlDbType.NVarChar) { Value = (object?)user.Profile.Description ?? DBNull.Value },
                        new SqlParameter("@ProfileImage", SqlDbType.NVarChar) { Value = (object?)user.Profile.ProfileImage ?? DBNull.Value },
                        };

                    int affectedRows = await _queryBuilder.ExecuteQueryAsync(profileQuery, profileParameters);
                    return affectedRows > 0;
                }
            }

            return false;
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            string query = @"
        UPDATE [Users]
        SET [FullName] = @FullName, [Email] = @Email
        WHERE [Id] = @UserId;

        UPDATE [UserProfiles]
        SET [PhoneNumber] = @PhoneNumber, [Passport] = @Passport, 
            [DrivingExperience] = @DrivingExperience, [Description] = @Description,
            [ProfileImage] = @ProfileImage
        WHERE [UserId] = @UserId;
        ";

            SqlParameter[] parameters = {
                new SqlParameter("@FullName", SqlDbType.NVarChar) { Value = user.FullName },
                new SqlParameter("@Email", SqlDbType.NVarChar) { Value = user.Email },
                new SqlParameter("@PhoneNumber", SqlDbType.NVarChar) { Value = user.Profile?.PhoneNumber ?? (object)DBNull.Value },
                new SqlParameter("@Passport", SqlDbType.NVarChar) { Value = user.Profile?.Passport ?? (object)DBNull.Value },
                new SqlParameter("@DrivingExperience", SqlDbType.Int) { Value = user.Profile?.DrivingExperience ?? (object)DBNull.Value },
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = user.Profile?.Description ?? (object)DBNull.Value },
                new SqlParameter("@ProfileImage", SqlDbType.NVarChar) { Value = user.Profile?.ProfileImage ?? (object)DBNull.Value },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = user.Id }
            };

            int affectedRows = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return affectedRows > 0;
        }
        public async Task<bool> UpdateUserWithRoleAsync(User user)
        {
            string query = @"
        UPDATE [Users]
        SET [FullName] = @FullName, [Email] = @Email, [RoleId] = @RoleId
        WHERE [Id] = @UserId;

        UPDATE [UserProfiles]
        SET [PhoneNumber] = @PhoneNumber, [Passport] = @Passport, 
            [DrivingExperience] = @DrivingExperience, [Description] = @Description,
            [ProfileImage] = @ProfileImage
        WHERE [UserId] = @UserId;
        ";

            SqlParameter[] parameters = {
                new SqlParameter("@FullName", SqlDbType.NVarChar) { Value = user.FullName },
                new SqlParameter("@Email", SqlDbType.NVarChar) { Value = user.Email },
                new SqlParameter("@RoleId", SqlDbType.Int) { Value = user.RoleId },
                new SqlParameter("@PhoneNumber", SqlDbType.NVarChar) { Value = user.Profile?.PhoneNumber ?? (object)DBNull.Value },
                new SqlParameter("@Passport", SqlDbType.NVarChar) { Value = user.Profile?.Passport ?? (object)DBNull.Value },
                new SqlParameter("@DrivingExperience", SqlDbType.Int) { Value = user.Profile?.DrivingExperience ?? (object)DBNull.Value },
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = user.Profile?.Description ?? (object)DBNull.Value },
                new SqlParameter("@ProfileImage", SqlDbType.NVarChar) { Value = user.Profile?.ProfileImage ?? (object)DBNull.Value },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = user.Id }
            };

            int affectedRows = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return affectedRows > 0;
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? user = null;
            string query = "SELECT [Id],[Email],[FullName],[RoleId] FROM [Users] WHERE [Id] = @Id";
            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                if (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        FullName = reader.GetString(2),
                        RoleId = reader.GetInt32(3)
                    };
                }
            }, new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            return user;
        }
        public async Task<User?> GetUserWithRoleByEmailAsync(string email)
        {
            User? user = null;
            string query = """
                SELECT u.[Id],u.[Email],u.[FullName],u.[RoleId],r.[Name] FROM [Users] u  
                JOIN [Roles] r ON r.[Id] = u.[RoleId]
                WHERE u.[Email] = @Email
                """;
            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                if (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        FullName = reader.GetString(2),
                        RoleId = reader.GetInt32(3),
                        Role = new Role
                        {
                            Name = reader.IsDBNull(4) ? null : reader.GetString(4)
                        }
                    };
                }
            }, new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email });
            return user;
        }
        public async Task<User?> GetUserWithUserProfileByIdAsync(int id)
        {
            User? user = null;

            string query = @"
        SELECT u.[Id], u.[Email], u.[FullName], u.[RoleId], 
               p.[Id] AS ProfileId, p.[PhoneNumber], p.[Passport], 
               p.[DrivingExperience], p.[Description], p.[ProfileImage]
        FROM [Users] u
        LEFT JOIN [UserProfiles] p ON u.[Id] = p.[UserId]
        WHERE u.[Id] = @UserId";

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                if (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        FullName = reader.GetString(2),
                        RoleId = reader.GetInt32(3),
                        Profile = reader.IsDBNull(4) ? null : new UserProfile
                        {
                            Id = reader.GetInt32(4),
                            PhoneNumber = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Passport = reader.IsDBNull(6) ? null : reader.GetString(6),
                            DrivingExperience = reader.GetInt32(7),
                            Description = reader.IsDBNull(8) ? null : reader.GetString(8),
                            ProfileImage = reader.IsDBNull(9) ? null : reader.GetString(9),
                            UserId = id
                        }
                    };
                }
            }, new SqlParameter("@UserId", SqlDbType.Int) { Value = id });
            return user;
        }
        public async Task<string?> GetUserProfileImageByEmailAsync(string email)
        {
            string? profileImage = null;
            string query = "SELECT p.[ProfileImage] FROM [Users] u LEFT JOIN [UserProfiles] p ON u.[Id] = p.[UserId] WHERE u.[Email] = @Email";
            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                if (reader.Read())
                {
                    profileImage = reader.IsDBNull(0) ? null : reader.GetString(0);
                }
            }, new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email });
            return profileImage;
        }
        public async Task<(IEnumerable<User> Users, int TotalCount)> GetCustomersWithProfileAsync(FilterModel filterModel)
        {
            var users = new List<User>();
            int totalCount = 0;

            string query = @"
    SELECT u.Id AS UserId, u.Email, u.FullName, u.RoleId, 
           p.Id AS ProfileId, p.PhoneNumber, p.Passport, 
           p.DrivingExperience, p.Description, p.ProfileImage
    FROM Users u
    LEFT JOIN UserProfiles p ON u.Id = p.UserId
    WHERE (@Search IS NULL OR u.FullName LIKE '%' + @Search + '%')
    ORDER BY u.Id
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*)
    FROM Users u
    LEFT JOIN UserProfiles p ON u.Id = p.UserId
    WHERE (@Search IS NULL OR u.FullName LIKE '%' + @Search + '%');";

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var user = new User
                    {
                        Id = reader.GetInt32("UserId"),
                        Email = reader.GetString("Email"),
                        FullName = reader.IsDBNull("FullName") ? null : reader.GetString("FullName"),
                        RoleId = reader.GetInt32("RoleId"),
                        Profile = reader.IsDBNull("ProfileId")
                                  ? null
                                  : new UserProfile
                                  {
                                      Id = reader.GetInt32("ProfileId"),
                                      PhoneNumber = reader.IsDBNull("PhoneNumber") ? null : reader.GetString("PhoneNumber"),
                                      Passport = reader.IsDBNull("Passport") ? null : reader.GetString("Passport"),
                                      DrivingExperience = reader.IsDBNull("DrivingExperience") ? 0 : reader.GetInt32("DrivingExperience"),
                                      Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                                      ProfileImage = reader.IsDBNull("ProfileImage") ? null : reader.GetString("ProfileImage"),
                                      UserId = reader.GetInt32("UserId")
                                  }
                    };

                    users.Add(user);
                }

                if (reader.NextResult() && reader.Read())
                {
                    totalCount = reader.GetInt32(0);
                }
            },
            new SqlParameter("@Search", string.IsNullOrWhiteSpace(filterModel.Search) ? (object)DBNull.Value : filterModel.Search),
            new SqlParameter("@Offset", (filterModel.Page - 1) * filterModel.PageSize),
            new SqlParameter("@PageSize", filterModel.PageSize));

            return (users, totalCount);
        }


        public async Task<bool> SigInAsync(User user)
        {
            string query = "SELECT [Salt] FROM [Users] WHERE [Email] = @Email";
            string? salt = null;
            SqlParameter emailParam = new SqlParameter("@Email", SqlDbType.VarChar) { Value = user.Email };
            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                if (reader.Read())
                {
                    salt = reader.GetString(0);
                }
            }, emailParam);

            if (salt is null)
            {
                return false;
            }
            string hashedPassword = SecurityHelper.HashPassword(user.HashPassword, salt, 10101, 70);
            query = "SELECT COUNT(*) FROM [Users] WHERE [Email] = @Email AND [HashPassword] = @HashPassword";
            emailParam = new SqlParameter("@Email", SqlDbType.VarChar) { Value = user.Email };
            SqlParameter hashPasswordParam = new SqlParameter("@HashPassword", SqlDbType.VarChar) { Value = hashedPassword };
            int affectedRows = await _queryBuilder.ExecuteScalarAsync<int>(query, emailParam, hashPasswordParam);
            return affectedRows > 0;
        }
        public async Task<bool> RegisterAsync(User user)
        {
            string salt = SecurityHelper.GenerateSalt(70);
            string hashedPassword = SecurityHelper.HashPassword(user.HashPassword, salt, 10101, 70);
            string query = @"
            INSERT INTO [Users] (Email, HashPassword, Salt, FullName, RoleId)
            OUTPUT INSERTED.Id
            VALUES (@Email, @HashPassword, @Salt, @FullName, @RoleId)";

            SqlParameter[] parameters = {
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = user.Email },
                new SqlParameter("@HashPassword", SqlDbType.VarChar) { Value = hashedPassword },
                new SqlParameter("@Salt", SqlDbType.VarChar) { Value = salt },
                new SqlParameter("@FullName", SqlDbType.VarChar) { Value = user.FullName },
                new SqlParameter("@RoleId", SqlDbType.Int) { Value = user.RoleId },
            };

            int userId = await _queryBuilder.ExecuteScalarAsync<int>(query, parameters);

            if (userId > 0)
            {
                string profileQuery = @"
                    INSERT INTO [UserProfiles] ([UserId],[DrivingExperience]) VALUES (@UserId,@DrivingExperience)";

                SqlParameter[] profileParameters = {
                        new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                        new SqlParameter("@DrivingExperience", SqlDbType.Int) { Value = 1 },
                        };

                int affectedRows = await _queryBuilder.ExecuteQueryAsync(profileQuery, profileParameters);
                return affectedRows > 0;
            }

            return false;
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            int count = await _queryBuilder.ExecuteScalarAsync<int>(query, new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email });
            return count > 0;
        }
        public async Task<bool> IsTheSameUserPasswordAsync(User user)
        {
            string? userHashPassword = null, userSalt = null;
            string query = "SELECT [HashPassword],[Salt] FROM [Users] WHERE [Id] = @Id";
            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                if (reader.Read())
                {
                    userHashPassword = reader.GetString(0);
                    userSalt = reader.GetString(1);
                }
            }, new SqlParameter("@Id", SqlDbType.Int) { Value = user.Id });

            string hashedPassword = SecurityHelper.HashPassword(user.HashPassword, userSalt, 10101, 70);
            return userHashPassword?.Equals(hashedPassword) ?? false;
        }
        public async Task<bool> ChangeUserPasswordAsync(User user)
        {
            string salt = SecurityHelper.GenerateSalt(70);
            string hashedPassword = SecurityHelper.HashPassword(user.HashPassword, salt, 10101, 70);

            string query = @"
                UPDATE [Users]
                SET [HashPassword] = @HashPassword, [Salt] = @Salt
                WHERE [Id] = @UserId;
                ";

            SqlParameter[] parameters = {
                new SqlParameter("@HashPassword", SqlDbType.NVarChar) { Value = hashedPassword },
                new SqlParameter("@Salt", SqlDbType.NVarChar) { Value = salt },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = user.Id }
            };

            int affectedRows = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return affectedRows > 0;
        }
        public async Task<bool> RemoveUserAsync(string userId)
        {
            string queryDelete = "DELETE FROM [Users] WHERE [Id] = @Id";
            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(queryDelete,
            new SqlParameter("@Id", SqlDbType.NVarChar) { Value = userId });
            return rowsAffected > 0;
        }

        public async Task<(IEnumerable<User> Users, int TotalCount)> GetUsersAsync(FilterModel filterModel, string? roleFilter = null)
        {
            var users = new List<User>();
            int totalCount = 0;

            string query = @"
                SELECT * FROM Users
                WHERE (@RoleFilter IS NULL OR Role = @RoleFilter)
                ORDER BY Id
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*) FROM Users
                WHERE (@RoleFilter IS NULL OR Role = @RoleFilter);";

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32("Id"),
                        FullName = reader.GetString("FullName"),
                        Email = reader.GetString("Email"),
                        RoleId = reader.GetInt32("RoleId"),
                    });
                }

                if (reader.NextResult() && reader.Read())
                {
                    totalCount = reader.GetInt32(0);
                }
            },
            new SqlParameter("@RoleFilter", string.IsNullOrWhiteSpace(roleFilter) ? (object)DBNull.Value : roleFilter),
            new SqlParameter("@Offset", (filterModel.Page - 1) * filterModel.PageSize),
            new SqlParameter("@PageSize", filterModel.PageSize));

            return (users, totalCount);
        }

        public bool IsValidFilter(FilterModel filterModel)
        {
            if (filterModel == null) return false;

            return filterModel.Page > 0 && filterModel.PageSize > 0;
        }
        public async Task<(IEnumerable<User> Users, int TotalCount)> GetAdminsAndEmployeesAsync(
      FilterModel? filterModel, int page, int pageSize, string? roleFilter)
        {
            var users = new List<User>();
            int totalCount = 0;

            // Формування списку ролей для фільтрації
            string roleCondition = "RoleId IN (1, 2)";
            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                roleCondition = $"RoleId = @RoleFilter";
            }

            string query = $@"
    SELECT Id, FullName, Email, RoleId FROM Users
    WHERE {roleCondition}
    ORDER BY Id
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM Users
    WHERE {roleCondition};";

            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Offset", (page - 1) * pageSize),
        new SqlParameter("@PageSize", pageSize)
    };

            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                parameters.Add(new SqlParameter("@RoleFilter", roleFilter));
            }

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        FullName = reader.GetString(reader.GetOrdinal("FullName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                    });
                }

                if (reader.NextResult() && reader.Read())
                {
                    totalCount = reader.GetInt32(0);
                }
            }, parameters.ToArray());

            return (users, totalCount);
        }



    }
}
