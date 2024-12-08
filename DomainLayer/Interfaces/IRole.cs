using RentalSystem.Models;

namespace RentalSystem.Interfaces
{
    public interface IRole
    {
        Task AddRoleAsync(Role role);
        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
}
