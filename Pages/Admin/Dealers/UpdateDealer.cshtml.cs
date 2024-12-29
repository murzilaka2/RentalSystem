using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Pages.Admin.Cars;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalSystem.Pages.Admin.Dealers
{
    public class UpdateDealerModel : PageModel
    {
        private readonly IDealer _dealers;
        public UpdateDealerModel(IDealer dealers)
        {
            _dealers = dealers;
        }
        [BindProperty]
        public string? ReturnUrl { get; set; }
        [BindProperty]
        public DealerViewModel DealerModel { get; set; }
        public Dealer CurrentDealer { get; set; }


        public async Task OnGetAsync(int id, string returnUrl)
        {
            ReturnUrl = returnUrl;

            CurrentDealer = await _dealers.GetDealerAsync(id);
            FillDealerProfile();
        }

        public async Task<IActionResult> OnPostAddDealerAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await _dealers.UpdateDealerAsync(new Dealer
            {
                Id = DealerModel.Id,
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

        private void FillDealerProfile()
        {
            DealerModel = new DealerViewModel
            {
                Id = CurrentDealer.Id,
                FirstName = CurrentDealer.FirstName,
                LastName = CurrentDealer.LastName,
                Email = CurrentDealer.Email,
                Fax = CurrentDealer.Fax,
                Mobile = CurrentDealer.Mobile,
                WhatsApp = CurrentDealer.WhatsApp,
                WorkExperience = CurrentDealer.WorkExperience
            };
        }

        public class DealerViewModel
        {
            [Key]
            public int Id { get; set; }
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
