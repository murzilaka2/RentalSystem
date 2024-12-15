using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RentalSystem.Pages.Auth
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required]
        [EmailAddress]
        [PageRemote(ErrorMessage = "No such Email address was found.", PageName = "Login", PageHandler = "IsEmailInUse")]
        public string? Email { get; set; }

        [BindProperty]
        [Required]
        [MinLength(6)]
        public string? Password { get; set; }

        [BindProperty]
        public string? ReturnUrl { get; set;}

        public IActionResult OnGet(string? returnUrl)
        {
            ReturnUrl = returnUrl;
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(ReturnUrl ?? "/");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromServices] IUser users)
        {
            if (ModelState.IsValid)
            {
                if (await users.SigInAsync(new Models.User
                {
                    Email = Email,
                    HashPassword = Password
                }))
                {
                    var currentUser = await users.GetUserWithRoleByEmailAsync(Email);
                    if (currentUser is null)
                    {
                        ModelState.AddModelError("", "User not found. Try again later.");
                        return Page();
                    }
                    //Добавление куки
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString()),
                        new Claim(ClaimTypes.Name, currentUser.Email),
                        new Claim(ClaimTypes.Role, currentUser.Role.Name),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    //Все прошло успешно, отправляем на защищеную страницу!
                    return Redirect(ReturnUrl ?? "/");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect Email or Password.");
                }
            }
            return Page();
        }

        public async Task<JsonResult> OnGetIsEmailInUseAsync([FromServices] IUser users, string Email)
        {
            if (!await users.IsEmailExistsAsync(Email))
            {
                return new JsonResult("No such Email address was found.");
            }
            return new JsonResult(true);
        }
    }
}
