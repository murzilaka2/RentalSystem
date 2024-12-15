using DomainLayer.Interfaces;
using DomainLayer.Models;
using RentalSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalSystem.Models;

namespace InfrastructureLayer.Repository
{
    public class WishListRepository : IWishList
    {
        private readonly QueryBuilder _queryBuilder;
        public WishListRepository(QueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public async Task<bool> AddWishListAsync(WishList wishList)
        {
            string query = @"
                INSERT INTO [WishesList] ([CarId], [UserId]) 
                VALUES (@CarId, @UserId)";

            SqlParameter[] parameters = {
                new SqlParameter("@CarId", SqlDbType.Int) { Value = wishList.CarId },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = wishList.UserId }
            };
            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<List<WishList>> GetWishesListWithCarsAsync(int userId)
        {
            string query = @"
                SELECT 
                    wl.[Id],
                    wl.[CreatedAt],
                    wl.[CarId],
                    wl.[UserId],
                    c.[Brand],
                    c.[Model],
                    c.[Year],
                    c.[Price],
                    c.[Image]
                FROM [WishesList] wl
                INNER JOIN [Cars] c ON wl.[CarId] = c.[Id]
                WHERE wl.[UserId] = @UserId";

            SqlParameter userIdParam = new SqlParameter("@UserId", SqlDbType.Int) { Value = userId };

            List<WishList> wishLists = new List<WishList>();

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var wishList = new WishList
                    {
                        Id = reader.GetInt32(0),
                        CreatedAt = reader.GetDateTime(1),
                        CarId = reader.GetInt32(2),
                        UserId = reader.GetInt32(3),
                        Car = new Car
                        {
                            Brand = reader.GetString(4),
                            Model = reader.GetString(5),
                            Year = reader.GetInt32(6),
                            Price = reader.GetDecimal(7),
                            Image = reader.IsDBNull(8) ? null : reader.GetString(8)
                        }
                    };

                    wishLists.Add(wishList);
                }
            }, userIdParam);

            return wishLists;
        }

        public async Task<bool> IsCarInWishListAsync(WishList wishList)
        {
            string query = "SELECT COUNT(1) FROM [WishesList] WHERE [CarId] = @CarId AND [UserId] = @UserId";

            SqlParameter carIdParam = new SqlParameter("@CarId", SqlDbType.Int) { Value = wishList.CarId };
            SqlParameter userIdParam = new SqlParameter("@UserId", SqlDbType.Int) { Value = wishList.UserId };
            int count = 0;
            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
            }, carIdParam, userIdParam);
            return count > 0;
        }


        public async Task<bool> RemoveWishListAsync(WishList wishList)
        {
            string queryDelete = "DELETE FROM [WishesList] WHERE [CarId] = @CarId AND [UserId] = @UserId";
            SqlParameter carIdParam = new SqlParameter("@CarId", SqlDbType.Int) { Value = wishList.CarId };
            SqlParameter userIdParam = new SqlParameter("@UserId", SqlDbType.Int) { Value = wishList.UserId };
            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(queryDelete, carIdParam, userIdParam);
            return rowsAffected > 0;
        }
    }
}
