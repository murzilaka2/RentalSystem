using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace RentalSystem.Pages.Admin.Users
{
    public class UpdateUserModel : PageModel
    {
        private readonly IUser _users;
        private readonly IRole _roles;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public UserProfileViewModel UserProfileModel { get; set; }

        [BindProperty]
        public string? ReturnUrl { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public User CurrentUser { get; set; }

        public UpdateUserModel(IUser users, IRole roles, IWebHostEnvironment environment)
        {
            _users = users;
            _roles = roles;
            _environment = environment;
        }

        private void FillUserProfile()
        {
            UserProfileModel = new UserProfileViewModel
            {
                Id = CurrentUser.Id,
                Description = CurrentUser.Profile.Description,
                DrivingExperience = CurrentUser.Profile.DrivingExperience,
                Email = CurrentUser.Email,
                FullName = CurrentUser.FullName,
                Passport = CurrentUser.Profile.Passport,
                PhoneNumber = CurrentUser.Profile.PhoneNumber,
                DisplayImage = CurrentUser.Profile.ProfileImage,
                RoleId = CurrentUser.RoleId
            };
        }

        public async Task OnGetAsync(int id, string returnUrl)
        {
            ReturnUrl = returnUrl;
            CurrentUser = await _users.GetUserWithUserProfileByIdAsync(id);
            Roles = await _roles.GetAllRolesAsync();
            FillUserProfile();
        }

        public async Task<IActionResult> OnPostUpdateUserAsync()
        {
            if (!ModelState.IsValid)
            {
                Roles = await _roles.GetAllRolesAsync();
                return Page();
            }

            User currentUser = new User
            {
                Id = UserProfileModel.Id,
                Email = UserProfileModel.Email,
                FullName = UserProfileModel.FullName,
                RoleId = UserProfileModel.RoleId.Value,
                Profile = new UserProfile
                {
                    PhoneNumber = UserProfileModel.PhoneNumber,
                    Passport = UserProfileModel.Passport,
                    DrivingExperience = UserProfileModel.DrivingExperience ?? 0,
                    Description = UserProfileModel.Description,
                    ProfileImage = UserProfileModel.DisplayImage
                }
            };

            //Проверка загруженного изображения
            if (UserProfileModel.ProfileImage != null)
            {
                //Удаление предыдущего изображения
                if (!string.IsNullOrEmpty(currentUser.Profile.ProfileImage))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, currentUser.Profile.ProfileImage);
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
                currentUser.Profile.ProfileImage = baseFolder + "/" + uniqueFileName;
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await UserProfileModel.ProfileImage.CopyToAsync(fileStream);
                }
            }

            if (await _users.UpdateUserWithRoleAsync(currentUser))
            {
                TempData["SuccessfullyChanged"] = true;
                return Redirect("/adminDashboard/updateUser?id=" + UserProfileModel.Id);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Data update error.");
                return Page();
            }
        }

        public class UserProfileViewModel
        {
            [Key]
            public int Id { get; set; }
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
        }
    }
}
