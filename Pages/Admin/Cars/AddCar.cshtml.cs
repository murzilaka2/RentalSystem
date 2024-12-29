using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Interfaces;
using RentalSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace RentalSystem.Pages.Admin.Cars
{
    public class AddCarModel : PageModel
    {
        private readonly ICar _cars;
        private readonly IDealer _dealers;
        private readonly IWebHostEnvironment _environment;

        public AddCarModel(ICar cars, IDealer dealers, IWebHostEnvironment environment)
        {
            _cars = cars;
            _dealers = dealers;
            _environment = environment;
        }

        [BindProperty]
        public CarViewModel CarModel { get; set; }
        public IEnumerable<Dealer> Dealers { get; set; }
        public async Task OnGetAsync()
        {
            Dealers = await _dealers.GetDealersAsync();
        }

        public async Task<IActionResult> OnPostAddCarAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Car currentCar = new Car
            {
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

            if (await _cars.AddCarAsync(currentCar))
            {
                TempData["SuccessfullyAdded"] = true;
                return Redirect("/adminDashboard/cars");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "When executing the request, an error occurred. Try again later.");
                return Page();
            }
        }

        public class CarViewModel
        {

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
