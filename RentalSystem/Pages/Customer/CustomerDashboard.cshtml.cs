using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;

namespace RentalSystem.Pages.Customer
{
    [Authorize(Roles = "Customer,Admin")]
    public class CustomerDashboardModel : PageModel
    {
        private readonly IRental _rentals;
        private readonly ICar _cars;
        public PaginationModel Pagination { get; set; }
        public List<Rental> Rentals { get; set; } = new List<Rental>();

        public CustomerDashboardModel(IRental rentals, ICar cars)
        {
            _rentals = rentals;
            _cars = cars;
        }
        public async Task<IActionResult> OnGetAsync([FromQuery] PaginationModel paginationModel)
        {

            string customerId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value ?? null;
            var (rentals, totalCount) = await _rentals.GetRentalsCustomerAsync(int.Parse(customerId), paginationModel);
            Rentals = rentals.ToList();

            Pagination = new PaginationModel(totalCount, paginationModel.Page, paginationModel.PageSize,
                Request.Path, paginationModel.Filter, paginationModel.Status)
            {
                SelectOptions = (await _cars.GetCarBrandAsync()).ToArray(),
                SortItem = paginationModel.SortItem,
                Desc = paginationModel.Desc
            };

            return Page();
        }
    }
}
