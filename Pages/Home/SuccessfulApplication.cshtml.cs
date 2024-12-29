using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RentalSystem.Pages.Home
{
    [AllowAnonymous]
    public class SuccessfulApplicationModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
