using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;

namespace RentalSystem.Pages.Admin.RentalCars
{
    public class RentalCarModel : PageModel
    {
        private readonly IRental _rentals;
        private readonly ICar _cars;
        public PaginationModel Pagination { get; set; }
        public List<Rental> Rentals { get; set; } = new List<Rental>();
        public RentalCarModel(IRental rentals, ICar cars)
        {
            _rentals = rentals;
            _cars = cars;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] PaginationModel paginationModel)
        {
            var (rentals, totcalCount) = await _rentals.GetRentalsAsync(paginationModel);
            Rentals = rentals.ToList();

            Pagination = new PaginationModel(totcalCount, paginationModel.Page, paginationModel.PageSize,
                Request.Path, paginationModel.Filter, paginationModel.Status)
            {
                SelectOptions = (await _cars.GetCarBrandAsync()).ToArray(),
                SortItem = paginationModel.SortItem,
                Desc = paginationModel.Desc
            };

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, string returnUrl)
        {
            if (id > 0 && await _rentals.RemoveRentalAsync(id))
            {
                TempData["SuccessfullyDeleted"] = "The rental has been successfully deleted.";
            }
            else
            {
                TempData["Error"] = "Failed to delete the rental. Try again later.";
            }
            return Redirect(returnUrl);
        }

    }
}
