using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RentalSystem.Pages.Shared.Pages
{
    public class WishListModel : PageModel
    {
        private readonly IWishList _wishList;
        public WishListModel(IWishList wishList)
        {
            _wishList = wishList;
        }
        public List<WishList> Wishes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value ?? null;
            Wishes = await _wishList.GetWishesListWithCarsAsync(int.Parse(userId));
            return Page();
        }
    }
}
