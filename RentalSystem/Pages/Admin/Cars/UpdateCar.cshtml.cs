using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;
using System.ComponentModel.DataAnnotations;


namespace RentalSystem.Pages.Admin.Cars
{
    public class UpdateCarModel : PageModel
    {
        private readonly ICar _cars;
        private readonly IDealer _dealers;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public string? ReturnUrl { get; set; }

        [BindProperty]
        public CarViewModel CarModel { get; set; }
        public Car CurrentCar { get; set; }
        public IEnumerable<Dealer> Dealers { get; set; }

        public UpdateCarModel(ICar cars, IDealer dealers, IWebHostEnvironment environment)
        {
            _cars = cars;
            _dealers = dealers;
            _environment = environment;
        }

        public async Task OnGetAsync(int id, string returnUrl)
        {
            ReturnUrl = returnUrl;
            Dealers = await _dealers.GetDealersAsync();
            CurrentCar = await _cars.GetCarAsync(id);
            FillCarProfile();
        }

        public async Task<IActionResult> OnPostUpdateCarAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Car currentCar = new Car
            {
                Id = CarModel.Id,
                Year = CarModel.Year ?? 2000,
                Transmission = CarModel.Transmission,
                SeatsCount = CarModel.SeatsCount,
                Price = CarModel.Price,
                Brand = CarModel.Brand,
                CarType = CarModel.CarType,
                Color = CarModel.Color,
                Image = CarModel.DisplayImage,
                CurrentMileage = CarModel.CurrentMileage,
                EngineDisplacement = CarModel.EngineDisplacement ?? 2.0,
                MileageLimit = CarModel.MileageLimit,
                Model = CarModel.Model,
                DealerId = CarModel.DealerId
            };

            //Проверка загруженного изображения
            if (CarModel.Image != null)
            {
                //Удаление предыдущего изображения
                if (!string.IsNullOrEmpty(currentCar.Image))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, currentCar.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string baseFolder = "assets/carsImages";
                var uploadsFolder = Path.Combine(_environment.WebRootPath, baseFolder);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + CarModel.Image.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                currentCar.Image = baseFolder + "/" + uniqueFileName;
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await CarModel.Image.CopyToAsync(fileStream);
                }
            }

            if (await _cars.UpdateCarAsync(currentCar))
            {
                TempData["SuccessfullyChanged"] = true;
                return Redirect("/adminDashboard/updateCar?id=" + CarModel.Id);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Data update error.");
                return Page();
            }
        }
        private void FillCarProfile()
        {
            CarModel = new CarViewModel
            {
                Id = CurrentCar.Id,
                Brand = CurrentCar.Brand,
                CarType = CurrentCar.CarType,
                Color = CurrentCar.Color,
                CurrentMileage = CurrentCar.CurrentMileage,
                DisplayImage = CurrentCar.Image,
                EngineDisplacement = CurrentCar.EngineDisplacement,
                MileageLimit = CurrentCar.MileageLimit,
                Model = CurrentCar.Model,
                Price = CurrentCar.Price,
                SeatsCount = CurrentCar.SeatsCount,
                Transmission = CurrentCar.Transmission,
                Year = CurrentCar.Year,
                DealerId = CurrentCar.DealerId
            };
        }

        public class CarViewModel
        {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "Brand is required.")]
            public string Brand { get; set; }

            [Required(ErrorMessage = "Model is required.")]
            public string? Model { get; set; }
            public string? Color { get; set; }

            [Required(ErrorMessage = "Engine Displacement is required.")]
            public double? EngineDisplacement { get; set; }

            [Required(ErrorMessage = "Year is required.")]
            [Range(1990, 2025, ErrorMessage = "Year must be between 1990 and 2025 years.")]
            public int? Year { get; set; }
            public CarType CarType { get; set; }
            public Transmission Transmission { get; set; }

            [Required(ErrorMessage = "Current Mileage is required.")]
            public double CurrentMileage { get; set; }
            //Лимит миль на аренду
            //Если -1, то без ограничения
            [Required(ErrorMessage = "Current Mileage is required. Indicate -1 if there are no restrictions.")]
            public int MileageLimit { get; set; }

            [Required(ErrorMessage = "Seats Count is required.")]
            [Range(1, 24, ErrorMessage = "Seats Count must be at least 1.")]
            public int SeatsCount { get; set; }

            //Цена в сутки

            [Required(ErrorMessage = "Price is required. Please specify price per day.")]
            public decimal Price { get; set; }

            public string? DisplayImage { get; set; }
            public IFormFile? Image { get; set; }
            public int DealerId { get; set; }
        }
    }
}
