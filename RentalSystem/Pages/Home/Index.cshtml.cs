using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;

namespace RentalSystem.Pages.Home
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ICar _cars;
        private readonly IDealer _dealers;
        public PaginationModel Pagination { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
        public int TotalCars { get; set; }
        public Dealer? Dealer { get; set; }
        public IndexModel(ICar cars, IDealer dealers)
        {
            _cars = cars;
            _dealers = dealers;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] PaginationModel paginationModel)
        {
            if (paginationModel.CarTypes != null)
            {
                paginationModel.CarTypesInt = paginationModel.CarTypes
                    .Select(ct => (int)Enum.Parse(typeof(CarType), ct))
                    .ToList();
            }

            var (cars, totalCars) = await _cars.GetAllClientCarsAsync(paginationModel);

            Cars = cars.ToList();
            TotalCars = totalCars;
            Pagination = new PaginationModel(totalCars, paginationModel.Page, paginationModel.PageSize,
                Request.Path, paginationModel.Filter, paginationModel.Status)
            {
                SelectOptions = (await _cars.GetCarBrandAsync(false)).ToArray(),
                SortItem = paginationModel.SortItem,
                Desc = paginationModel.Desc,
                PriceRange = paginationModel.PriceRange,
                CarTypesInt = paginationModel.CarTypesInt,
                CarTypes = paginationModel.CarTypes,
                CarBrands = paginationModel.CarBrands
            };

            if (paginationModel.DealerId != null && paginationModel.DealerId > 0)
            {
                Dealer = await _dealers.GetDealerAsync(paginationModel.DealerId.Value);
            }

            return Page();
        }
    }
}
