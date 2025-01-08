using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;

namespace RentalSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        private readonly ICar _cars;
        public List<CarRentalInfo> CarRentalInfos { get; set; }
        public PaginationModel CarRentalPagination { get; set; } 
        public int TotalCarRentalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int CarRentalPage { get; set; } = 1;

        public AdminDashboardModel(ICar cars)
        {
            _cars = cars;
        }

        //Доделать поиск и пагинацию
        //Сделать тестовые данные для списка желаний
        //Сделать тестовые данные для тест драйвов

        public async Task<IActionResult> OnGetAsync([FromQuery] PaginationModel carRentalPaginationModel, [FromQuery] int carRentalPage = 1)
        {
            var (carRentals, totalCarRentals) = await _cars.GetMostRentedCarsAsync(new FilterModel
            {
                Filter = carRentalPaginationModel.Filter,
                Status = carRentalPaginationModel.Status == "All" ? null : carRentalPaginationModel.Status,
                Page = carRentalPaginationModel.Page,
                PageSize = carRentalPaginationModel.PageSize
            });

            CarRentalInfos = carRentals.ToList();
            CarRentalPagination = new PaginationModel(totalCarRentals, carRentalPaginationModel.Page, carRentalPaginationModel.PageSize,
                Request.Path, carRentalPaginationModel.Filter, carRentalPaginationModel.Status)
            {
                SelectOptions = new string[] { "Model" }
            };
            TotalCarRentalPages = (int)Math.Ceiling((double)totalCarRentals / PageSize);
            CarRentalPage = carRentalPage;          
            return Page();
        }
    }

}
