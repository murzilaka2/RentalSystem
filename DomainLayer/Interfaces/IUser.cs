using DomainLayer.Models;
using RentalSystem.Models;

namespace RentalSystem.Interfaces
{
    public interface IUser
    {
        Task<(IEnumerable<User> Users, int TotalCount)> GetAllUsersWithProfileAndRoleAsync(FilterModel filterModel);
        Task<(IEnumerable<User> Users, int TotalCount)> GetUsersAsync(FilterModel filterModel, string? roleFilter = null);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserWithUserProfileByIdAsync(int id);
        Task<User?> GetUserWithRoleByEmailAsync(string email);
        Task<string?> GetUserProfileImageByEmailAsync(string email);
        Task<(IEnumerable<User> Users, int TotalCount)> GetCustomersWithProfileAsync(FilterModel filterModel);
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> UpdateUserWithRoleAsync(User user);
        Task<bool> RemoveUserAsync(string userId);

        Task<bool> SigInAsync(User user);
        Task<bool> RegisterAsync(User user);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsTheSameUserPasswordAsync(User user);
        Task<bool> ChangeUserPasswordAsync(User user);

        Task<(IEnumerable<User> Users, int TotalCount)> GetAdminsAndEmployeesAsync(FilterModel? filterModel, int page, int pageSize, string? roleFilter);
        bool IsValidFilter(FilterModel filterModel);
    }
}
