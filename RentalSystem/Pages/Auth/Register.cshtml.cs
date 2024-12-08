using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;


namespace RentalSystem.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        [Required]
        public string? FullName { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        [PageRemote(ErrorMessage = "Email address already exists", PageName = "Register", PageHandler = "IsEmailInUse")]
        public string? Email { get; set; }

        [BindProperty]
        [Required]
        [MinLength(6)]
        public string? Password { get; set; }


        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromServices] IUser users)
        {
            if (ModelState.IsValid)
            {
                if (await users.IsEmailExistsAsync(Email))
                {
                    ModelState.AddModelError("Register.Email", "Email address is already in use.");
                    return Page();
                }

                if (await users.RegisterAsync(new Models.User
                {
                    Email = Email,
                    FullName = FullName,
                    HashPassword = Password,
                    RoleId = 3
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
                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred, please try again later.");
                }
            }
            return Page();
        }

        public async Task<JsonResult> OnGetIsEmailInUse([FromServices] IUser users, [FromQuery] string email)
        {
            if (await users.IsEmailExistsAsync(email))
            {
                return new JsonResult("Email address is already in use.");
            }
            return new JsonResult(true);
        }
    }
}
