using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RentalSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        private readonly IReview _reviews;
        public List<Review> Reviews { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; } = 3;
        public int Page { get; set; } = 1;
        public AdminDashboardModel(IReview reviews)
        {
            _reviews = reviews;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] int page = 1)
        {
            (Reviews, TotalItems) = (await _reviews.GetAllReviewsAsync(page, PageSize));
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
            Page = page;
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, string returnUrl)
        {
            if (id > 0 && await _reviews.RemoveReviewAsync(id))
            {
                TempData["SuccessfullyDeleted"] = "The review has been successfully deleted.";
            }
            else
            {
                TempData["Error"] = "Failed to delete the review. Try again later.";
            }
            return Redirect(returnUrl);
        }
    }
}
