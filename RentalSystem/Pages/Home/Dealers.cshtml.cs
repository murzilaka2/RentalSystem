using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Models;

namespace RentalSystem.Pages.Home
{
    public class DealersModel : PageModel
    {
        private readonly IDealer _dealers;
        public PaginationModel Pagination { get; set; }
        public List<Dealer> Dealers { get; set; } = new List<Dealer>();

        public DealersModel(IDealer dealers)
        {
            _dealers = dealers;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] PaginationModel paginationModel)
        {
            var (dealers, totalDealers) = await _dealers.GetAllDealersWithCarsCountAsync(new FilterModel
            {
                Filter = paginationModel.Filter,
                Status = paginationModel.Status == "All" ? null : paginationModel.Status,
                Page = paginationModel.Page,
                PageSize = paginationModel.PageSize
            });

            Dealers = dealers.ToList();

            Pagination = new PaginationModel(totalDealers, paginationModel.Page, paginationModel.PageSize,
                Request.Path, paginationModel.Filter, paginationModel.Status)
            {
                SelectOptions = new string[] { "Work Expirience", "First Name", "Last Name" }
            };
            return Page();
        }
    }
}
