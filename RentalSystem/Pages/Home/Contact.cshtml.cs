using EmailService;
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

        public IActionResult OnPostAsync([FromServices] IEmailSender emailSender)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Сделать загрузку получателей:

            var message = new Message(new string[] { "enykoruna1@gmail.com" }, "Rental System - Contact Form", $"""
                Letter from: {ContactForm.Email}
                First Name: {ContactForm.FirstName}
                Phone Number: {ContactForm.PhoneNumber}
                Message: {ContactForm.Message}
                """);
            if (emailSender.SendEmail(message))
            {
                TempData["Info"] = "Your email has been successfully sent!";
            }
            else
            {
                TempData["Error"] = "An error occurred while sending the email. Please try again later.";
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
