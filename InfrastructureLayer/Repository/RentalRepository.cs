using DomainLayer.Interfaces;
using DomainLayer.Models;
using RentalSystem.Services;
using System.Data.SqlClient;
using System.Data;
using RentalSystem.Models;

namespace InfrastructureLayer.Repository
{
    public class RentalRepository : IRental
    {
        private readonly QueryBuilder _queryBuilder;

        public RentalRepository(QueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public async Task<bool> RentCarAsync(Rental rental)
        {
            string query = @"INSERT INTO [Rentals] (
                        [StartDate],[EndDate], [StartMileage],[IsGPSNavigationSystem],
                        [ChildSeat],[AdditionalDriver],[InsuranceCoverage],[CarId],
                        [UserId]
                     )
                     OUTPUT INSERTED.Id
                     VALUES (
                        @StartDate,@EndDate,@StartMileage,@IsGPSNavigationSystem,
                        @ChildSeat,@AdditionalDriver,@InsuranceCoverage,@CarId,
                        @UserId
                     )";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = rental.StartDate },
                new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = (object)rental.EndDate ?? DBNull.Value },
                new SqlParameter("@StartMileage", SqlDbType.Float) { Value = rental.StartMileage },
                new SqlParameter("@IsGPSNavigationSystem", SqlDbType.Bit) { Value = rental.IsGPSNavigationSystem },
                new SqlParameter("@ChildSeat", SqlDbType.Bit) { Value = rental.ChildSeat },
                new SqlParameter("@AdditionalDriver", SqlDbType.Bit) { Value = rental.AdditionalDriver },
                new SqlParameter("@InsuranceCoverage", SqlDbType.Bit) { Value = rental.InsuranceCoverage },
                new SqlParameter("@CarId", SqlDbType.Int) { Value = rental.CarId },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = rental.UserId }
            };

            int rentalId = await _queryBuilder.ExecuteScalarAsync<int>(query, parameters);
            return rentalId > 0;
        }
        public async Task<(IEnumerable<Rental> Rentals, int TotalCount)> GetRentalsAsync(PaginationModel pagination)
        {
            //Сортировка по алиасу
            if (pagination.SortItem is null || pagination.SortItem.Equals("StartDate"))
            {
                pagination.SortItem = "StartDate";
            }
            string sortDirection = pagination.Desc ? "DESC" : "ASC";

            string query = $@"
            WITH FilteredRentals AS (
                SELECT 
                    r.[Id] AS RentalId,              
                    r.[StartDate],
                    r.[EndDate],
                    r.[StartMileage],
                    r.[EndMileage],
                    r.[IsGPSNavigationSystem],
                    r.[ChildSeat],
                    r.[AdditionalDriver],
                    r.[InsuranceCoverage],
                    r.[CarId],
                    r.[Status],
                    r.[UserId] AS RentalUserId,      
                    u.[Id] AS UserId,                
                    u.[Email],
                    u.[FullName],
                    c.[Brand],
                    c.[Model],
                    c.[Image],
                    c.[Price]
                FROM [Rentals] r
                INNER JOIN [Cars] c ON r.CarId = c.Id
                INNER JOIN [Users] u ON r.UserId = u.Id
                WHERE (@Filter IS NULL OR (c.Brand LIKE '%' + @Filter + '%' OR c.Model LIKE '%' + @Filter + '%'))
                AND (@Status = 'ALL' OR @Status IS NULL OR c.Brand = @Status)
            )
            SELECT 
                *, 
                COUNT(*) OVER() AS TotalCount
            FROM FilteredRentals
            ORDER BY [{pagination.SortItem}] {sortDirection}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            List<SqlParameter> parameters = new List<SqlParameter>()
    {
        new SqlParameter("@Filter", SqlDbType.NVarChar) { Value = (object)pagination.Filter ?? DBNull.Value },
        new SqlParameter("@Status", SqlDbType.NVarChar) { Value = (object)pagination.Status ?? DBNull.Value },
        new SqlParameter("@Offset", SqlDbType.Int) { Value = (pagination.Page - 1) * pagination.PageSize },
        new SqlParameter("@PageSize", SqlDbType.Int) { Value = pagination.PageSize }
    };

            List<Rental> rentals = new List<Rental>();
            int totalCount = 0;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var rental = new Rental
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("RentalId")),         
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndDate")),
                        StartMileage = reader.GetDouble(reader.GetOrdinal("StartMileage")),
                        EndMileage = reader.IsDBNull(reader.GetOrdinal("EndMileage")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("EndMileage")),
                        IsGPSNavigationSystem = reader.GetBoolean(reader.GetOrdinal("IsGPSNavigationSystem")),
                        ChildSeat = reader.GetBoolean(reader.GetOrdinal("ChildSeat")),
                        AdditionalDriver = reader.GetBoolean(reader.GetOrdinal("AdditionalDriver")),
                        InsuranceCoverage = reader.GetBoolean(reader.GetOrdinal("InsuranceCoverage")),
                        Status = (RentalHistoryStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                        Car = new Car
                        {
                            Brand = reader.GetString(reader.GetOrdinal("Brand")),
                            Model = reader.GetString(reader.GetOrdinal("Model")),
                            Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                        },
                        User = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("UserId")),      
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? null : reader.GetString(reader.GetOrdinal("FullName"))
                        }
                    };
                    rentals.Add(rental);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
                    }
                }
            }, parameters.ToArray());

            return (rentals, totalCount);
        }
        public async Task<bool> RemoveRentalAsync(int rentalId)
        {
            string queryDelete = "DELETE FROM [Rentals] WHERE [Id] = @Id";
            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(queryDelete,
            new SqlParameter("@Id", SqlDbType.NVarChar) { Value = rentalId });
            return rowsAffected > 0;
        }
        public async Task<Rental> GetRentalAsync(int rentalId)
        {
            string query = $@"
        SELECT 
            r.[Id] AS RentalId,              
            r.[StartDate],
            r.[EndDate],
            r.[StartMileage],
            r.[EndMileage],
            r.[IsGPSNavigationSystem],
            r.[ChildSeat],
            r.[AdditionalDriver],
            r.[InsuranceCoverage],
            r.[Status],
            r.[PaymentStatus],
            r.[CarId],
            r.[UserId] AS RentalUserId,      
            u.[Id] AS UserId,                
            u.[Email],
            u.[FullName],
            c.[Brand],
            c.[Model],
            c.[Image],
            c.[Price],
            c.[Color],
            c.[Year],
            c.[RentalStatus],
            up.[PhoneNumber],
            up.[Passport],
            up.[DrivingExperience],
            up.[Description],
            up.[ProfileImage]
        FROM [Rentals] r
        INNER JOIN [Cars] c ON r.CarId = c.Id
        INNER JOIN [Users] u ON r.UserId = u.Id
        LEFT JOIN [UserProfiles] up ON u.Id = up.UserId
        WHERE r.[Id] = @Id
    ";

            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Id", SqlDbType.Int) { Value = rentalId }
    };

            Rental rental = null;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                if (reader.Read())
                {
                    rental = new Rental
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("RentalId")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndDate")),
                        StartMileage = reader.GetDouble(reader.GetOrdinal("StartMileage")),
                        EndMileage = reader.IsDBNull(reader.GetOrdinal("EndMileage")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("EndMileage")),
                        IsGPSNavigationSystem = reader.GetBoolean(reader.GetOrdinal("IsGPSNavigationSystem")),
                        ChildSeat = reader.GetBoolean(reader.GetOrdinal("ChildSeat")),
                        AdditionalDriver = reader.GetBoolean(reader.GetOrdinal("AdditionalDriver")),
                        InsuranceCoverage = reader.GetBoolean(reader.GetOrdinal("InsuranceCoverage")),
                        Status = (RentalHistoryStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                        PaymentStatus = (PaymentStatus)reader.GetInt32(reader.GetOrdinal("PaymentStatus")),
                        Car = new Car
                        {
                            Brand = reader.GetString(reader.GetOrdinal("Brand")),
                            Model = reader.GetString(reader.GetOrdinal("Model")),
                            Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Color = reader.GetString(reader.GetOrdinal("Color")),
                            Year = reader.GetInt32(reader.GetOrdinal("Year")),
                            RentalStatus = (RentalStatus)reader.GetInt32(reader.GetOrdinal("RentalStatus"))
                        },
                        User = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? null : reader.GetString(reader.GetOrdinal("FullName")),
                            Profile = new UserProfile
                            {
                                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Passport = reader.IsDBNull(reader.GetOrdinal("Passport")) ? null : reader.GetString(reader.GetOrdinal("Passport")),
                                DrivingExperience = reader.GetInt32(reader.GetOrdinal("DrivingExperience")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                ProfileImage = reader.IsDBNull(reader.GetOrdinal("ProfileImage")) ? null : reader.GetString(reader.GetOrdinal("ProfileImage"))
                            }
                        }
                    };
                }
            }, parameters.ToArray());

            return rental;
        }
        public async Task<bool> UpdateRentalAsync(Rental rental)
        {
            string query = @"
        UPDATE [Rentals]
        SET 
            [StartDate] = @StartDate,
            [EndDate] = @EndDate,
            [StartMileage] = @StartMileage,
            [EndMileage] = @EndMileage,
            [IsGPSNavigationSystem] = @IsGPSNavigationSystem,
            [ChildSeat] = @ChildSeat,
            [AdditionalDriver] = @AdditionalDriver,
            [InsuranceCoverage] = @InsuranceCoverage
        WHERE 
            [Id] = @RentalId AND
            [CarId] = @CarId AND
            [UserId] = @UserId
    ";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = rental.StartDate },
                new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = (object)rental.EndDate ?? DBNull.Value },
                new SqlParameter("@StartMileage", SqlDbType.Float) { Value = rental.StartMileage },
                new SqlParameter("@EndMileage", SqlDbType.Float) { Value = (object)rental.EndMileage ?? DBNull.Value },
                new SqlParameter("@IsGPSNavigationSystem", SqlDbType.Bit) { Value = rental.IsGPSNavigationSystem },
                new SqlParameter("@ChildSeat", SqlDbType.Bit) { Value = rental.ChildSeat },
                new SqlParameter("@AdditionalDriver", SqlDbType.Bit) { Value = rental.AdditionalDriver },
                new SqlParameter("@InsuranceCoverage", SqlDbType.Bit) { Value = rental.InsuranceCoverage },
                new SqlParameter("@RentalId", SqlDbType.Int) { Value = rental.Id },
                new SqlParameter("@CarId", SqlDbType.Int) { Value = rental.CarId },  
                new SqlParameter("@UserId", SqlDbType.Int) { Value = rental.UserId }   
            };

            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }
        public async Task<bool> CancelRentalAsync(int rentalId)
        {
            string query = @"
            UPDATE [Rentals]
            SET 
                [Status] = @Status
            WHERE 
                [Id] = @RentalId
            ";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Status", SqlDbType.Int) { Value = (int)RentalHistoryStatus.Canceled }, 
                new SqlParameter("@RentalId", SqlDbType.Int) { Value = rentalId }  
            };

            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }
        public async Task<bool> CloseRentalAsync(int rentalId)
        {
            string query = @"
            UPDATE [Rentals]
            SET 
                [Status] = @Status
            WHERE 
                [Id] = @RentalId
            ";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Status", SqlDbType.Int) { Value = (int)RentalHistoryStatus.Completed },
                new SqlParameter("@RentalId", SqlDbType.Int) { Value = rentalId }
            };

            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }
        public async Task<(IEnumerable<Rental> Rentals, int TotalCount)> GetRentalsCustomerAsync(int customerId, PaginationModel pagination)
        {
            //Сортировка по алиасу
            if (pagination.SortItem is null || pagination.SortItem.Equals("StartDate"))
            {
                pagination.SortItem = "StartDate";
            }
            string sortDirection = pagination.Desc ? "DESC" : "ASC";

            string query = $@"
            WITH FilteredRentals AS (
                SELECT 
                    r.[Id] AS RentalId,              
                    r.[StartDate],
                    r.[EndDate],
                    r.[StartMileage],
                    r.[EndMileage],
                    r.[IsGPSNavigationSystem],
                    r.[ChildSeat],
                    r.[AdditionalDriver],
                    r.[InsuranceCoverage],
                    r.[CarId],
                    r.[Status],
                    r.[UserId] AS RentalUserId,      
                    u.[Id] AS UserId,                
                    u.[Email],
                    u.[FullName],
                    c.[Brand],
                    c.[Model],
                    c.[Image],
                    c.[Price],
                    c.[CurrentMileage],
                    c.[Year]
                FROM [Rentals] r
                INNER JOIN [Cars] c ON r.CarId = c.Id
                INNER JOIN [Users] u ON r.UserId = u.Id
                WHERE (@UserId = r.UserId) AND (@Filter IS NULL OR (c.Brand LIKE '%' + @Filter + '%' OR c.Model LIKE '%' + @Filter + '%'))
                AND (@Status = 'ALL' OR @Status IS NULL OR c.Brand = @Status)
            )
            SELECT 
                *, 
                COUNT(*) OVER() AS TotalCount
            FROM FilteredRentals
            ORDER BY [{pagination.SortItem}] {sortDirection}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@UserId", SqlDbType.Int) { Value = customerId },
                new SqlParameter("@Filter", SqlDbType.NVarChar) { Value = (object)pagination.Filter ?? DBNull.Value },
                new SqlParameter("@Status", SqlDbType.NVarChar) { Value = (object)pagination.Status ?? DBNull.Value },
                new SqlParameter("@Offset", SqlDbType.Int) { Value = (pagination.Page - 1) * pagination.PageSize },
                new SqlParameter("@PageSize", SqlDbType.Int) { Value = pagination.PageSize }
            };

            List<Rental> rentals = new List<Rental>();
            int totalCount = 0;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var rental = new Rental
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("RentalId")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndDate")),
                        StartMileage = reader.GetDouble(reader.GetOrdinal("StartMileage")),
                        EndMileage = reader.IsDBNull(reader.GetOrdinal("EndMileage")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("EndMileage")),
                        IsGPSNavigationSystem = reader.GetBoolean(reader.GetOrdinal("IsGPSNavigationSystem")),
                        ChildSeat = reader.GetBoolean(reader.GetOrdinal("ChildSeat")),
                        AdditionalDriver = reader.GetBoolean(reader.GetOrdinal("AdditionalDriver")),
                        InsuranceCoverage = reader.GetBoolean(reader.GetOrdinal("InsuranceCoverage")),
                        Status = (RentalHistoryStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                        Car = new Car
                        {
                            Brand = reader.GetString(reader.GetOrdinal("Brand")),
                            Model = reader.GetString(reader.GetOrdinal("Model")),
                            Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            CurrentMileage = reader.GetDouble(reader.GetOrdinal("CurrentMileage")),
                            Year = reader.GetInt32(reader.GetOrdinal("Year"))
                        },
                        User = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? null : reader.GetString(reader.GetOrdinal("FullName"))
                        }
                    };
                    rentals.Add(rental);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
                    }
                }
            }, parameters.ToArray());

            return (rentals, totalCount);
        }
    }
}
