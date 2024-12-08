using DomainLayer.Models;
using RentalSystem.Models;

namespace RentalSystem.Interfaces
{
    public interface IUser
    {
        Task<(IEnumerable<User> Users, int TotalCount)> GetAllUsersWithProfileAndRoleAsync(FilterModel filterModel);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserWithUserProfileByIdAsync(int id);
        Task<User?> GetUserWithRoleByEmailAsync(string email);
        Task<string?> GetUserProfileImageByEmailAsync(string email);
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> UpdateUserWithRoleAsync(User user);
        Task<bool> SigInAsync(User user);
        Task<bool> RegisterAsync(User user);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsTheSameUserPasswordAsync(User user);
        Task<bool> ChangeUserPasswordAsync(User user);
        Task<bool> RemoveUserAsync(string userId);
    }
}
