using System.ComponentModel.DataAnnotations;

namespace RentalSystem.Models
{
    public enum CarType
    {
        Sedan,
        Suv,
        Crossover,
        [Display(Name = "Pickup Truck")]
        PickupTruck,
        Hatchback,
        Convertible,
        Luxury,
        Coupe,
        Minivan,
        [Display(Name = "Sport Car")]
        SportCar
    }
}
