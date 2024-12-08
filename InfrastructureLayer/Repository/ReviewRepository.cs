using DomainLayer.Interfaces;
using DomainLayer.Models;
using System.Data.SqlClient;
using System.Data;
using RentalSystem.Services;
using RentalSystem.Models;

namespace InfrastructureLayer.Repository
{
    public class ReviewRepository : IReview
    {
        private readonly QueryBuilder _queryBuilder;
        public ReviewRepository(QueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public async Task<bool> AddReviewAsync(Review review)
        {
            string query = @"INSERT INTO [Reviews] ([Rating], [Message], [UserId], [CarId]) 
                     VALUES (@Rating, @Message, @UserId, @CarId)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Rating", SqlDbType.Int) { Value = review.Rating },
                new SqlParameter("@Message", SqlDbType.NVarChar) { Value = review.Message },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = review.UserId },
                new SqlParameter("@CarId", SqlDbType.Int) { Value = review.CarId }
            };

            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<(IEnumerable<Review> Reviews, int TotalCount, double AverageRating)> GetCarReviewsAsync(int carId, int page, int pageSize = 10)
        {
            string query = @"
            WITH FilteredReviews AS (
                SELECT 
                    r.[Id],
                    r.[CreatedAt],
                    r.[Rating],
                    r.[Message],
                    r.[UserId],
                    r.[CarId]
                FROM [Reviews] r
                WHERE r.CarId = @CarId
            ),
            ReviewStats AS (
                SELECT AVG(CAST(r.[Rating] AS FLOAT)) AS AverageRating
                FROM [Reviews] r
                WHERE r.CarId = @CarId
            )
            SELECT 
                r.Id, 
                r.CreatedAt, 
                r.Rating, 
                r.Message, 
                r.UserId, 
                r.CarId,  
                u.FullName,  
                up.ProfileImage,  
                COUNT(*) OVER() AS TotalCount,
                rs.AverageRating
            FROM FilteredReviews r
            LEFT JOIN [Users] u ON r.UserId = u.Id
            LEFT JOIN [UserProfiles] up ON r.UserId = up.UserId
            CROSS JOIN ReviewStats rs
            ORDER BY r.CreatedAt DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            SqlParameter[] parameters = {
                new SqlParameter("@CarId", SqlDbType.Int) { Value = carId },
                new SqlParameter("@Offset", SqlDbType.Int) { Value = (page - 1) * pageSize },
                new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize }
            };

            List<Review> reviews = new List<Review>();
            int totalCount = 0;
            double averageRating = 0;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var review = new Review
                    {
                        Id = reader.GetInt32(0),
                        CreatedAt = reader.GetDateTime(1),
                        Rating = reader.GetInt32(2),
                        Message = reader.GetString(3),
                        UserId = reader.GetInt32(4),
                        CarId = reader.GetInt32(5),
                        User = new RentalSystem.Models.User
                        {
                            FullName = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Profile = new RentalSystem.Models.UserProfile
                            {
                                ProfileImage = reader.IsDBNull(7) ? null : reader.GetString(7)
                            }
                        }
                    };
                    reviews.Add(review);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetInt32(8);
                    }

                    if (averageRating == 0)
                    {
                        averageRating = reader.IsDBNull(9) ? 0 : reader.GetDouble(9);
                    }
                }
            }, parameters);

            return (reviews, totalCount, averageRating);
        }
        public async Task<(List<Review> Reviews, int TotalCount)> GetAllReviewsAsync(int page, int pageSize = 10)
        {
            string query = @"
                SELECT 
                    r.Id, 
                    r.CreatedAt, 
                    r.Rating, 
                    r.Message, 
                    r.UserId, 
                    r.CarId,
                    u.FullName, 
                    up.ProfileImage
                FROM [Reviews] r
                LEFT JOIN [Users] u ON r.UserId = u.Id
                LEFT JOIN [UserProfiles] up ON r.UserId = up.UserId
                ORDER BY r.CreatedAt DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            string countQuery = "SELECT COUNT(*) FROM [Reviews];";

            SqlParameter[] parameters = {
                 new SqlParameter("@Offset", SqlDbType.Int) { Value = (page - 1) * pageSize },
                 new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize }
                };

            List<Review> reviews = new List<Review>();
            int totalCount = 0;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var review = new Review
                    {
                        Id = reader.GetInt32(0),
                        CreatedAt = reader.GetDateTime(1),
                        Rating = reader.GetInt32(2),
                        Message = reader.GetString(3),
                        UserId = reader.GetInt32(4),
                        CarId = reader.GetInt32(5),
                        User = new RentalSystem.Models.User
                        {
                            FullName = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Profile = new RentalSystem.Models.UserProfile
                            {
                                ProfileImage = reader.IsDBNull(7) ? null : reader.GetString(7)
                            }
                        }
                    };
                    reviews.Add(review);
                }
            }, parameters);

            await _queryBuilder.ExecuteQueryAsync(countQuery, reader =>
            {
                if (reader.Read())
                {
                    totalCount = reader.GetInt32(0);
                }
            });

            return (reviews, totalCount);
        }
        public async Task<(List<Review> Reviews, int TotalCount)> GetClientReviewsAsync(int userId, int page, int pageSize = 10)
        {
            string query = @"
                SELECT 
                    r.Id, 
                    r.CreatedAt, 
                    r.Rating, 
                    r.Message, 
                    r.UserId, 
                    r.CarId,
                    u.FullName, 
                    up.ProfileImage
                FROM [Reviews] r
                LEFT JOIN [Users] u ON r.UserId = u.Id
                LEFT JOIN [UserProfiles] up ON r.UserId = up.UserId
                WHERE r.UserId = @UserId
                ORDER BY r.CreatedAt DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            string countQuery = "SELECT COUNT(*) FROM [Reviews];";

            SqlParameter[] parameters = {
                 new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                 new SqlParameter("@Offset", SqlDbType.Int) { Value = (page - 1) * pageSize },
                 new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize }
                };

            List<Review> reviews = new List<Review>();
            int totalCount = 0;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var review = new Review
                    {
                        Id = reader.GetInt32(0),
                        CreatedAt = reader.GetDateTime(1),
                        Rating = reader.GetInt32(2),
                        Message = reader.GetString(3),
                        UserId = reader.GetInt32(4),
                        CarId = reader.GetInt32(5),
                        User = new RentalSystem.Models.User
                        {
                            FullName = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Profile = new RentalSystem.Models.UserProfile
                            {
                                ProfileImage = reader.IsDBNull(7) ? null : reader.GetString(7)
                            }
                        }
                    };
                    reviews.Add(review);
                }
            }, parameters);

            await _queryBuilder.ExecuteQueryAsync(countQuery, reader =>
            {
                if (reader.Read())
                {
                    totalCount = reader.GetInt32(0);
                }
            });

            return (reviews, totalCount);
        }
        public async Task<bool> RemoveReviewAsync(int id)
        {
            string queryDelete = "DELETE FROM [Reviews] WHERE [Id] = @Id";
            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(queryDelete,
            new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            return rowsAffected > 0;
        }
    }
}
