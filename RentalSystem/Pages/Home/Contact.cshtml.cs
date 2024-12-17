using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace RentalSystem.Pages.Home
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public ContactFormViewModel ContactForm { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostAsync()
        {
            //Дописать получение формы, вывод сообщения об отправке или ошибке.
            //Добавить конфигурационный файл и считывать почту на которуб отправляем от туда
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return Page();
        }

        public class ContactFormViewModel
        {
            [Required(ErrorMessage = "First Name is required.")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Phone number is required.")]
            [Phone(ErrorMessage = "Invalid phone number.")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Email address is required.")]
            [EmailAddress(ErrorMessage = "Invalid email address.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Message is required.")]
            public string Message { get; set; }

            [Required(ErrorMessage = "You must agree to the terms.")]
            public bool AgreeToTerms { get; set; }
        }
    }
}
