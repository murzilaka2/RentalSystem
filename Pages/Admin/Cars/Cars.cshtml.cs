using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;

namespace RentalSystem.Pages.Admin.Cars
{
    public class CarsModel : PageModel
    {
        private readonly ICar _cars;
        public PaginationModel Pagination { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
        public CarsModel(ICar cars)
        {
            _cars = cars;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] PaginationModel paginationModel)
        {
            var (cars, totalUsers) = await _cars.GetAllCarsAsync(new FilterModel
            {
                Filter = paginationModel.Filter,
                Status = paginationModel.Status == "All" ? null : paginationModel.Status,
                Page = paginationModel.Page,
                PageSize = paginationModel.PageSize
            });

            Cars = cars.ToList();

            Pagination = new PaginationModel(totalUsers, paginationModel.Page, paginationModel.PageSize,
                Request.Path, paginationModel.Filter, paginationModel.Status)
            {
                SelectOptions = (await _cars.GetCarBrandAsync()).ToArray()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, string returnUrl)
        {
            if (id > 0 && await _cars.RemoveCarAsync(id))
            {
                TempData["SuccessfullyDeleted"] = "The car has been successfully deleted.";
            }
            else
            {
                TempData["Error"] = "Failed to delete the car. Try again later.";
            }
            return Redirect(returnUrl);
        }
    }
}
