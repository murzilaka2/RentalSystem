using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;

namespace RentalSystem.Pages.Admin.Dealers
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
     
            var (dealers, totalUsers) = await _dealers.GetAllDealersAsync(new FilterModel
            {
                Filter = paginationModel.Filter,
                Status = paginationModel.Status == "All" ? null : paginationModel.Status,
                Page = paginationModel.Page,
                PageSize = paginationModel.PageSize
            });

            Dealers = dealers.ToList();

            Pagination = new PaginationModel(totalUsers, paginationModel.Page, paginationModel.PageSize,
                Request.Path, paginationModel.Filter, paginationModel.Status)
            {
                SelectOptions = new string[] {"Work Expirience", "First Name", "Last Name" }
            };
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, string returnUrl)
        {
            if (id > 0 && await _dealers.RemoveDealerAsync(id))
            {
                TempData["SuccessfullyDeleted"] = "The dealer has been successfully deleted.";
            }
            else
            {
                TempData["Error"] = "Failed to delete the dealer. Try again later.";
            }
            return Redirect(returnUrl);
        }
    }
}
