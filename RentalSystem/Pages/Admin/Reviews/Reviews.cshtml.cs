using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Models;

namespace RentalSystem.Pages.Admin.Reviews
{
    public class ReviewsModel : PageModel
    {
        private readonly IReview _reviews;

        public ReviewsModel(IReview reviews)
        {
            _reviews = reviews;
        }

        public List<Review> Reviews { get; set; }
        public PaginationModel ReviewPagination { get; set; }
        public int TotalReviewPages { get; set; }
        public int ReviewPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public async Task<IActionResult> OnGetAsync([FromQuery] PaginationModel reviewPaginationModel, [FromQuery] int reviewPage = 1)
        {
            (Reviews, int totalReviews) = await _reviews.GetAllReviewsAsync(reviewPage, PageSize);
            ReviewPagination = new PaginationModel(totalReviews, reviewPaginationModel.Page, reviewPaginationModel.PageSize,
                Request.Path, reviewPaginationModel.Filter, reviewPaginationModel.Status)
            {
                SelectOptions = new string[] { "Rating" }
            };
            TotalReviewPages = (int)Math.Ceiling((double)totalReviews / PageSize);
            ReviewPage = reviewPage;

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
