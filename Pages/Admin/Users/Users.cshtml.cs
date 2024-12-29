using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;

namespace RentalSystem.Pages.Admin.Users
{
    public class UsersModel : PageModel
    {
        private readonly IUser _users;
        public PaginationModel Pagination { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public UsersModel(IUser users)
        {
            _users = users;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] PaginationModel paginationModel)
        {
            var (users, totalUsers) = await _users.GetAllUsersWithProfileAndRoleAsync(new FilterModel
            {
                Filter = paginationModel.Filter,
                Status = paginationModel.Status == "All" ? null : paginationModel.Status,
                Page = paginationModel.Page,
                PageSize = paginationModel.PageSize
            });

            Users = users.ToList();

            Pagination = new PaginationModel(totalUsers, paginationModel.Page, paginationModel.PageSize,
                Request.Path, paginationModel.Filter, paginationModel.Status)
            {
                SelectOptions = new string[] { "All", "Admin", "Employee", "Customer" }
            };
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id, string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(id) && await _users.RemoveUserAsync(id))
            {
                TempData["SuccessfullyDeleted"] = "The user has been successfully deleted.";
            }
            else
            {
                TempData["Error"] = "Failed to delete the user. Try again later.";
            }
            return Redirect(returnUrl);
        }
    }
}
