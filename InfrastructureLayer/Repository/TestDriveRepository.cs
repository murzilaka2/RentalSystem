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
    public class TestDriveRepository : ITestDrive
    {
        private readonly QueryBuilder _queryBuilder;
        public TestDriveRepository(QueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public async Task<bool> AddTestDriveAsync(TestDrive testDrive)
        {
            string query = @"
                INSERT INTO [TestDrives] ([Name], [Phone], [Date], [CarId]) 
                VALUES (@Name, @Phone, @Date, @CarId)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Name", SqlDbType.NVarChar) { Value = testDrive.Name },
                new SqlParameter("@Phone", SqlDbType.NVarChar) { Value = testDrive.Phone },
                new SqlParameter("@Date", SqlDbType.DateTime) { Value = testDrive.Date },
                new SqlParameter("@CarId", SqlDbType.Int) { Value = testDrive.CarId }
            };

            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }
        public async Task<bool> RemoveTestDriveAsync(int testDriveId)
        {
            string queryDelete = "DELETE FROM [TestDrives] WHERE [Id] = @Id";
            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(queryDelete,
            new SqlParameter("@Id", SqlDbType.NVarChar) { Value = testDriveId });
            return rowsAffected > 0;
        }
        public async Task<bool> ChangeTestDriveStatusAsync(int testDriveId, TestDriveStatus testDriveStatus)
        {
            string query = @"
                UPDATE [TestDrives]
                SET [TestDriveStatus] = @TestDriveStatus
                WHERE [Id] = @TestDriveId;
            ";

            SqlParameter[] parameters =
            {
                new SqlParameter("@TestDriveId", SqlDbType.Int) { Value = testDriveId },
                new SqlParameter("@TestDriveStatus", SqlDbType.Int) { Value = (int)testDriveStatus }
            };
            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }
        public async Task<(IEnumerable<TestDrive> TestDrives, int TotalCount)> GetAllTestDrivesAsync(FilterModel filterModel)
        {
            List<TestDrive> testDrives = new List<TestDrive>();
            int totalCount = 0;


            string sortOrder = string.IsNullOrEmpty(filterModel.Status) ? "td.[Date] DESC" : $"td.[{filterModel.Status}]";
            if (filterModel.Status != null && filterModel.Status.Equals("Car Model"))
            {
                sortOrder = $"c.[Model]";
            }

            string query = $@"
            WITH PagedTestDrives AS (
                SELECT td.[Id], td.[Name], td.[Phone], td.[Date], 
                       td.[CarId], td.[TestDriveStatus], c.[Id] AS Car_Id, c.[Model] AS CarModel,
                       COUNT(*) OVER() AS TotalCount
                FROM [TestDrives] td
                LEFT JOIN [Cars] c ON td.[CarId] = c.[Id]
                WHERE (@Filter IS NULL OR td.[Name] LIKE @Filter OR td.[Phone] LIKE @Filter
                OR c.[Model] LIKE @Filter)
                ORDER BY {sortOrder}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            )
            SELECT * FROM PagedTestDrives;
            ";

            SqlParameter[] parameters =
            {
                new SqlParameter("@PageSize", SqlDbType.Int) { Value = filterModel.PageSize },
                new SqlParameter("@Offset", SqlDbType.Int) { Value = (filterModel.Page - 1) * filterModel.PageSize },
                new SqlParameter("@Filter", SqlDbType.NVarChar)
                {
                    Value = string.IsNullOrEmpty(filterModel.Filter) ? DBNull.Value : $"%{filterModel.Filter}%"
                }
            };

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    testDrives.Add(new TestDrive
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Phone = reader.GetString(2),
                        Date = reader.GetDateTime(3),
                        CarId = reader.GetInt32(4),
                        TestDriveStatus = (TestDriveStatus)reader.GetInt32(5),
                        Car = reader.IsDBNull(6) ? null : new Car
                        {
                            Id = reader.GetInt32(6),
                            Model = reader.GetString(7)
                        }
                    });
                    totalCount = reader.GetInt32(reader.FieldCount - 1);
                }
            }, parameters);

            return (testDrives, totalCount);
        }
    }
}
