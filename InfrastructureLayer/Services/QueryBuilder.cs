using RentalSystem.Interfaces;
using System.Data.SqlClient;
using DomainLayer;

namespace RentalSystem.Services
{
    public class QueryBuilder
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public QueryBuilder(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
        public async Task ExecuteQueryAsync(string query, Action<SqlDataReader> action)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStringProvider.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    action(reader);
                }
            }
        }

        public async Task ExecuteQueryAsync(string query, Action<SqlDataReader> action, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStringProvider.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        action(reader);
                    }
                }
            }
        }

        public async Task<int> ExecuteQueryAsync(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStringProvider.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected;
                }
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStringProvider.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);
                    object result = await command.ExecuteScalarAsync();
                    return (T)result;
                }
            }
        }


    }
}
