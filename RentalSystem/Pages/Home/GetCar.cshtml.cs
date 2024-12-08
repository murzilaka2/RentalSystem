using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RentalSystem.Pages.Home
{
    [AllowAnonymous]
    public class GetCarModel : PageModel
    {
        private readonly ICar _cars;
        private readonly IReview _review;
        public Car CurrentCar { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public int TotalReviews { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; } = 2;
        public int TotalPages { get; set; }
        public double AverageRating { get; set; }

        [BindProperty]
        public RentCarViewModel RentCar { get; set; }

        [BindProperty]
        public ReviewViewModel Review { get; set; }

        [BindProperty]
        public string? ReturnUrl { get; set; }
        public GetCarModel(ICar cars, IReview review)
        {
            _cars = cars;
            _review = review;
        }

        public async Task OnGetAsync(int id, string returnUrl, [FromQuery] int page = 1)
        {
            ReturnUrl = returnUrl;
            CurrentCar = await _cars.GetCarAsync(id);
            Page = page;

            (Reviews, TotalReviews, AverageRating) = await _review.GetCarReviewsAsync(id, page, PageSize);
            TotalPages = (int)Math.Ceiling((double)TotalReviews / PageSize);

            //You can add tracking of online users.
            TempData["Info"] = $"Right now there are {id} people watching this car.";
        }

        public async Task<IActionResult> OnPostRentFormAsync([FromServices] IRental rentalService)
        {
            if (RentCar.TotalToPay > 0)
            {
                CurrentCar = await _cars.GetCarAsync(RentCar.CarId);
                if (CurrentCar == null)
                {
                    Error();
                    return Redirect("/getCar?id=" + RentCar.CarId);
                }
                string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userId = 0;
                if (userStringId == null || !int.TryParse(userStringId, out userId))
                {
                    Error();
                    return Redirect("/getCar?id=" + RentCar.CarId);
                }

                Rental rental = new Rental
                {
                    UserId = userId,
                    CarId = RentCar.CarId,
                    StartDate = RentCar.StartDate,
                    EndDate = RentCar.EndDate,
                    AdditionalDriver = RentCar.AdditionalDriver,
                    ChildSeat = RentCar.ChildSeat,
                    IsGPSNavigationSystem = RentCar.IsGPSNavigationSystem,
                    InsuranceCoverage = RentCar.InsuranceCoverage,
                    StartMileage = CurrentCar.CurrentMileage,
                };
                if (!await rentalService.RentCarAsync(rental))
                {
                    Error();
                    return Redirect("/getCar?id=" + RentCar.CarId);
                }
                return Redirect("/successfulApplication");
            }
            return Redirect("/getCar?id=" + RentCar.CarId);
        }

        public async Task<IActionResult> OnPostReviewFormAsync()
        {
            CurrentCar = await _cars.GetCarAsync(Review.CarId);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = 0;
            if (userStringId == null || !int.TryParse(userStringId, out userId))
            {
                Error();
                return Redirect("/getCar?id=" + CurrentCar.Id + "#reviewArea");
            }

            if (await _review.AddReviewAsync(new Review
            {
                CarId = CurrentCar.Id,
                Message = Review.Message,
                Rating = Review.Rate,
                UserId = userId
            }))
            {
                Success("Your comment, successfully added!");
            }
            else
            {
                Error("When adding a comment, an error occurred. Please try again.");
            }

            return Redirect("/getCar?id=" + CurrentCar.Id + "#reviewArea");
        }
        private void Error(string? message = null)
        {
            if (message == null)
            {
                message = "While executing the query, an exception occurred. Please reload the page.";
            }
            TempData["Error"] = message;
        }
        private void Success(string message)
        {
            TempData["Success"] = message;
        }

        public class RentCarViewModel
        {
            public int CarId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal TotalToPay { get; set; }
            public bool IsGPSNavigationSystem { get; set; }
            public bool ChildSeat { get; set; }
            public bool AdditionalDriver { get; set; }
            public bool InsuranceCoverage { get; set; }
        }

        public class ReviewViewModel
        {
            public int CarId { get; set; }
            public int Rate { get; set; }

            [Required(ErrorMessage = "Write your review about this auto.")]
            public string? Message { get; set; }
        }
    }
}
