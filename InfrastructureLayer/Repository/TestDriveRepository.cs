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

        public Task<(List<TestDrive> Reviews, int TotalCount)> GetAllTestDrivesAsync(int page, int pageSize = 10)
        {
            throw new NotImplementedException();
        }
    }
}
