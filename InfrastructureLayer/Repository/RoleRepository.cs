using RentalSystem.Interfaces;
using RentalSystem.Models;
using RentalSystem.Services;
using System.Data.SqlClient;
using System.Data;

namespace RentalSystem.Repository
{
    public class RoleRepository : IRole
    {
        private readonly QueryBuilder _queryBuilder;

        public RoleRepository(QueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public async Task AddRoleAsync(Role role)
        {
            string query = "INSERT INTO [Roles] ([Name]) VALUES (@Name)";
            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.VarChar) { Value = role.Name };
            await _queryBuilder.ExecuteQueryAsync(query, reader => { }, nameParam);
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            string query = "SELECT [Id],[Name] FROM [Roles]";
            List<Role> roles = new List<Role>();

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var role = new Role
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };
                    roles.Add(role);
                }
            });

            return roles;
        }
    }
}
