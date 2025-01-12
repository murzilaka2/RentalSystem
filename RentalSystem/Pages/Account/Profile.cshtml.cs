using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RentalSystem.Pages.Account
{
    public class ProfileModel : PageModel
    {
        public readonly IUser _users;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public UserProfileViewModel UserProfileModel { get; set; }

        public ChangePasswordViewModel ChangePasswordModel { get; set; }

        public ProfileModel(IUser users, IWebHostEnvironment environment)
        {
            _users = users;
            _environment = environment;
        }

        public User? CurrentUser { get; set; }

        private async Task<bool> FillUser()
        {
            if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                CurrentUser = await _users.GetUserWithUserProfileByIdAsync(userId);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void FillUserProfile()
        {
            UserProfileModel = new UserProfileViewModel
            {
                Description = CurrentUser.Profile.Description,
                DrivingExperience = CurrentUser.Profile.DrivingExperience,
                Email = CurrentUser.Email,
                FullName = CurrentUser.FullName,
                Passport = CurrentUser.Profile.Passport,
                PhoneNumber = CurrentUser.Profile.PhoneNumber,
                DisplayImage = CurrentUser.Profile.ProfileImage
            };
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (await FillUser())
            {
                FillUserProfile();
                return Page();
            }
            else
            {
                return Redirect("/");
            }
        }
        public async Task<IActionResult> OnPostBaseFormAsync()
        {
            await FillUser();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            CurrentUser.FullName = UserProfileModel.FullName;
            CurrentUser.Email = UserProfileModel.Email;
            if (CurrentUser.Profile != null)
            {
                CurrentUser.Profile.PhoneNumber = UserProfileModel.PhoneNumber;
                CurrentUser.Profile.Passport = UserProfileModel.Passport;
                CurrentUser.Profile.DrivingExperience = UserProfileModel.DrivingExperience ?? 0;
                CurrentUser.Profile.Description = UserProfileModel.Description;
            }

            //Проверка загруженного изображения
            if (UserProfileModel.ProfileImage != null)
            {
                //Удаление предыдущего изображения
                if (!string.IsNullOrEmpty(CurrentUser.Profile.ProfileImage))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, CurrentUser.Profile.ProfileImage);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string baseFolder = "assets/usersImages";
                var uploadsFolder = Path.Combine(_environment.WebRootPath, baseFolder);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + UserProfileModel.ProfileImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                CurrentUser.Profile.ProfileImage = baseFolder + "/" + uniqueFileName;
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await UserProfileModel.ProfileImage.CopyToAsync(fileStream);
                }
            }

            if (await _users.UpdateUserAsync(CurrentUser))
            {
                TempData["SuccessfullyChanged"] = true;
                return RedirectToPage("Profile");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Data update error.");
                return Page();
            }
        }
        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            await FillUser();

            ChangePasswordModel = new ChangePasswordViewModel
            {
                NewPassword = Request.Form["ChangePasswordModel.NewPassword"],
                ConfirmPassword = Request.Form["ChangePasswordModel.ConfirmPassword"],
            };

            if (CurrentUser != null)
            {
                CurrentUser.HashPassword = ChangePasswordModel.NewPassword;

                if (await _users.IsTheSameUserPasswordAsync(CurrentUser))
                {
                    TempData["ErrorPasswordChanged"] = "Provide a new password.";
                    FillUserProfile();
                    return Page();
                }

                if (await _users.ChangeUserPasswordAsync(CurrentUser))
                {
                    TempData["SuccessfullyPasswordChanged"] = true;
                    return RedirectToPage("Profile");
                }
                else
                {
                    TempData["ErrorPasswordChanged"] = "There's been a mistake. Try again later.";
                    FillUserProfile();
                    return Page();
                }
            }
            return RedirectToPage("Profile");
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

            [Range(0, 100, ErrorMessage = "Driving experience must be between 0 and 100 years.")]
            public int? DrivingExperience { get; set; }

            public string? Description { get; set; }
            public IFormFile? ProfileImage { get; set; }
            public string? DisplayImage { get; set; }
        }
        public class ChangePasswordViewModel
        {
            [Required(ErrorMessage = "Enter a new password.")]
            [StringLength(100, ErrorMessage = "The password must contain a minimum of {2} and a maximum of {1} characters.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "Confirm the new password.")]
            [DataType(DataType.Password)]
            [Compare("NewPassword", ErrorMessage = "The passwords don't match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}
