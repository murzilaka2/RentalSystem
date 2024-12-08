using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace RentalSystem.Pages.Customer
{
    [Authorize(Roles = "Customer,Admin")]
    public class RentDetailsModel : PageModel
    {
        private readonly IRental _rentals;
        [BindProperty]
        public string? ReturnUrl { get; set; }

        [BindProperty]
        public RentalViewModel RentalModel { get; set; }
        public Rental CurrentRental { get; set; }

        public RentDetailsModel(IRental rentals)
        {
            _rentals = rentals;
        }

        public async Task OnGetAsync(int id, string returnUrl)
        {
            ReturnUrl = returnUrl;
            CurrentRental = await _rentals.GetRentalAsync(id);
            RentalModel = new RentalViewModel
            {
                Id = CurrentRental.Id,
                UserId = CurrentRental.User.Id,
                StartDate = CurrentRental.StartDate,
                EndDate = CurrentRental.EndDate,
                StartMileage = CurrentRental.StartMileage,
                EndMileage = CurrentRental.EndMileage,
                AdditionalDriver = CurrentRental.AdditionalDriver,
                ChildSeat = CurrentRental.ChildSeat,
                InsuranceCoverage = CurrentRental.InsuranceCoverage,
                IsGPSNavigationSystem = CurrentRental.IsGPSNavigationSystem,
                RentalHistoryStatus = CurrentRental.Status,
                PaymentStatus = CurrentRental.PaymentStatus
            };
        }

        public async Task<IActionResult> OnPostUpdateRentalAsync()
        {
            string action = Request.Form["action"];

            if (action.Equals("CancelLease"))
            {
                await _rentals.CancelRentalAsync(RentalModel.Id);
                TempData["SuccessfullyChanged"] = true;
                return Redirect("/customerDashboard/rentdetails?id=" + RentalModel.Id);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            CurrentRental = new Rental
            {
                Id = RentalModel.Id,
                AdditionalDriver = RentalModel.AdditionalDriver,
                ChildSeat = RentalModel.ChildSeat,
                InsuranceCoverage = RentalModel.InsuranceCoverage,
                IsGPSNavigationSystem = RentalModel.IsGPSNavigationSystem,
                StartDate = RentalModel.StartDate,
                EndDate = RentalModel.EndDate,
                StartMileage = RentalModel.StartMileage,
                EndMileage = RentalModel.EndMileage,
                CarId = RentalModel.Id,
                UserId = RentalModel.UserId
            };

            if (await _rentals.UpdateRentalAsync(CurrentRental))
            {
                TempData["SuccessfullyChanged"] = true;
                return Redirect("/customerDashboard/rentdetails?id=" + RentalModel.Id);
            }
            else
            {
                CurrentRental = await _rentals.GetRentalAsync(RentalModel.Id);
                ModelState.AddModelError(string.Empty, "Data update error.");
                return Page();
            }
        }

        public class RentalViewModel
        {
            [Key]
            public int Id { get; set; }
            [Key]
            public int UserId { get; set; }

            [Required]
            public double StartMileage { get; set; }

            [Required]
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public double? EndMileage { get; set; }

            public bool IsGPSNavigationSystem { get; set; }
            public bool ChildSeat { get; set; }
            public bool AdditionalDriver { get; set; }
            public bool InsuranceCoverage { get; set; }
            public RentalHistoryStatus RentalHistoryStatus { get; set; }
            public PaymentStatus PaymentStatus { get; set; }

            public decimal CalculateTotalDays()
            {
                if (EndDate.HasValue)
                {
                    TimeSpan dateDifference = EndDate.Value - StartDate;
                    return (decimal)dateDifference.TotalDays;
                }
                return 0;
            }
        }
    }
}
