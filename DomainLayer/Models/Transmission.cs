using System.ComponentModel.DataAnnotations;

namespace RentalSystem.Models
{
    public enum Transmission
    {
        Manual, 
        Automatic,
        [Display(Name = "Semi-Automatic")]
        SemiAutomatic,
        [Display(Name = "Dual Clutch")]
        DualClutch
    }
}
