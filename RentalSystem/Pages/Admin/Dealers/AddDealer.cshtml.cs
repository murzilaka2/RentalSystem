using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace RentalSystem.Pages.Admin.Dealers
{
    public class AddDealerModel : PageModel
    {
        private readonly IDealer _dealers;

        public AddDealerModel(IDealer dealers)
        {
            _dealers = dealers;
        }

        [BindProperty]
        public DealerViewModel DealerModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAddDealerAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await _dealers.AddDealerAsync(new Dealer
            {
                FirstName = DealerModel.FirstName,
                LastName = DealerModel.LastName,
                WorkExperience = DealerModel.WorkExperience,
                Email = DealerModel.Email,
                Mobile = DealerModel.Mobile,
                Fax = DealerModel.Fax,
                WhatsApp = DealerModel.WhatsApp
            }))
            {
                TempData["SuccessfullyAdded"] = true;
                return Redirect("/adminDashboard/dealers");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "When executing the request, an error occurred. Try again later.");
                return Page();
            }
        }

        public class DealerViewModel
        {
            [Required(ErrorMessage = "First Name is required.")]
            public string? FirstName { get; set; }
            
            [Required(ErrorMessage = "Last Name is required.")]
            public string? LastName { get; set; }
            public int WorkExperience { get; set; } = 0;

            [Required(ErrorMessage = "Mobile is required.")]
            public string? Mobile { get; set; }

            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress]
            public string? Email { get; set; }
            public string? WhatsApp { get; set; }
            public string? Fax { get; set; }
        }
    }
}
