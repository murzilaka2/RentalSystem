using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Helpers;
using RentalSystem.Interfaces;
using RentalSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace RentalSystem.Pages.Admin.Users
{
    public class AddUserModel : PageModel
    {
        private readonly IUser _users;
        private readonly IRole _roles;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public UserProfileViewModel UserProfileModel { get; set; }
        public IEnumerable<Role> Roles { get; set; }

        public AddUserModel(IUser users, IWebHostEnvironment environment, IRole roles)
        {
            _users = users;
            _environment = environment;
            _roles = roles;
        }
        public async Task OnGetAsync()
        {
            Roles = await _roles.GetAllRolesAsync();
        }

        public async Task<IActionResult> OnPostAddUserAsync()
        {
            if (!ModelState.IsValid)
            {
                Roles = await _roles.GetAllRolesAsync();
                return Page();
            }

            string salt = SecurityHelper.GenerateSalt(70);
            string hashedPassword = SecurityHelper.HashPassword(UserProfileModel.Password, salt, 10101, 70);

            User currentUser = new User
            {
                Email = UserProfileModel.Email,
                FullName = UserProfileModel.FullName,
                HashPassword = hashedPassword,
                RoleId = UserProfileModel.RoleId.Value,
                Profile = new UserProfile
                {
                    PhoneNumber = UserProfileModel.PhoneNumber,
                    Passport = UserProfileModel.Passport,
                    DrivingExperience = UserProfileModel.DrivingExperience ?? 0,
                    Description = UserProfileModel.Description
                }
            };

            //Проверка загруженного изображения
            if (UserProfileModel.ProfileImage != null)
            {              
                string baseFolder = "assets/usersImages";
                var uploadsFolder = Path.Combine(_environment.WebRootPath, baseFolder);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + UserProfileModel.ProfileImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                currentUser.Profile.ProfileImage = baseFolder + "/" + uniqueFileName;
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await UserProfileModel.ProfileImage.CopyToAsync(fileStream);
                }
            }

            if (await _users.AddUserAsync(currentUser))
            {
                TempData["SuccessfullyChanged"] = true;
                return RedirectToPage();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Data update error.");
                return Page();
            }
        }

        public class UserProfileViewModel
        {
            [Required(ErrorMessage = "Full Name is required.")]
            [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
            public string? FullName { get; set; }

            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid Email Address.")]
            public string? Email { get; set; }

            [Phone(ErrorMessage = "Invalid phone number.")]
            public string? PhoneNumber { get; set; }

            public string? Passport { get; set; }

            [Required(ErrorMessage = "Driving experience is required.")]
            [Range(0, 100, ErrorMessage = "Driving experience must be between 0 and 100 years.")]
            public int? DrivingExperience { get; set; }

            public string? Description { get; set; }

            [Required(ErrorMessage = "Role is required.")]
            public int? RoleId { get; set; }
            public IFormFile? ProfileImage { get; set; }
            public string? DisplayImage { get; set; }

            [Required(ErrorMessage = "Enter a password.")]
            [StringLength(100, ErrorMessage = "The password must contain a minimum of {2} and a maximum of {1} characters.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "Confirm the new password.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The passwords don't match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}

