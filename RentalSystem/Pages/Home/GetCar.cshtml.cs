using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace RentalSystem.Pages.Home
{
    [AllowAnonymous]
    public class GetCarModel : PageModel
    {
        private readonly ICar _cars;
        private readonly IReview _review;
        private readonly IWishList _wishList;
        public Car CurrentCar { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public int TotalReviews { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; } = 2;
        public int TotalPages { get; set; }
        public double AverageRating { get; set; }
        public bool IsCarInWishList { get; set; }

        [BindProperty]
        public RentCarViewModel RentCar { get; set; }

        [BindProperty]
        public ReviewViewModel Review { get; set; }

        [BindProperty]
        public TestDriveViewModel TestDrive { get; set; }

        [BindProperty]
        public string? ReturnUrl { get; set; }
        public GetCarModel(ICar cars, IReview review, IWishList wishList)
        {
            _cars = cars;
            _review = review;
            _wishList = wishList;
        }

        public async Task<IActionResult> OnGetAsync(int id, string returnUrl, [FromQuery] int page = 1)
        {
            ReturnUrl = returnUrl;
            CurrentCar = await _cars.GetCarAsync(id);
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = 0;
            if (userStringId is null)
            {
                IsCarInWishList = false;
            }
            else if (!int.TryParse(userStringId, out userId))
            {
                Error();
                return Redirect("/getCar?id=" + CurrentCar.Id);
            }
            IsCarInWishList = await _wishList.IsCarInWishListAsync(new WishList
            {
                CarId = id,
                UserId = userId
            });
            Page = page;

            (Reviews, TotalReviews, AverageRating) = await _review.GetCarReviewsAsync(id, page, PageSize);
            TotalPages = (int)Math.Ceiling((double)TotalReviews / PageSize);

            //You can add tracking of online users.
            TempData["Info"] = $"Right now there are {id} people watching this car.";
            return Page();
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
        public async Task<IActionResult> OnPostWishListFormAsync()
        {          
            string? userStringId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? null;
            int userId = 0;
            if (userStringId == null)
            {
                return Redirect("/login?returnUrl=getCar?id=" + RentCar.CarId);
            }
            else if (!int.TryParse(userStringId, out userId))
            {
                Error();
                return Redirect("/getCar?id=" + RentCar.CarId);
            }

            IsCarInWishList = await _wishList.IsCarInWishListAsync(new WishList
            {
                CarId = RentCar.CarId,
                UserId = userId
            });
            if (IsCarInWishList)
            {
                try
                {
                    if (await _wishList.RemoveWishListAsync(new WishList
                    {
                        CarId = RentCar.CarId,
                        UserId = userId
                    }))
                    {
                        Success("Auto has been successfully removed from your wish list.", "WishSuccess");
                    }
                    else
                    {
                        Error(name: "WishError");
                    }
                }
                catch (Exception)
                {
                    Error(name: "WishError");
                }
            }
            else
            {
                try
                {
                    if (await _wishList.AddWishListAsync(new WishList
                    {
                        CarId = RentCar.CarId,
                        UserId = userId
                    }))
                    {
                        Success("Auto has been successfully added to your wish list.", "WishSuccess");
                    }
                    else
                    {
                        Error(name: "WishError");
                    }
                }
                catch (Exception)
                {
                    Error(name: "WishError");
                }
            }          
            return Redirect("/getCar?id=" + RentCar.CarId);
        }

        public async Task<IActionResult> OnPostTestDriveFormAsync([FromServices] ITestDrive testDrive)
        {
            //Добавить объект TestDrive в базу данных
            if (await testDrive.AddTestDriveAsync(new DomainLayer.Models.TestDrive
            {
                Name = TestDrive.Name,
                Phone = TestDrive.Phone,    
                Date = TestDrive.Date,
                CarId = TestDrive.CarId              
            }))
            {
                Success("Your request has been sent successfully. Expect a call from the manager.", "WishSuccess");
            }
            else
            {
                Error(name: "WishError");
            }
            return Redirect("/getCar?id=" + TestDrive.CarId);
        }
        private void Error(string? message = null, string name = "Error")
        {
            if (message == null)
            {
                message = "While executing the query, an exception occurred. Please reload the page.";
            }
            TempData[name] = message;
        }
        private void Success(string message, string name = "Success")
        {
            TempData[name] = message;
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

        public class TestDriveViewModel
        {
            public int CarId { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
