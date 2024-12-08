using RentalSystem.Models;


namespace DomainLayer.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //Пробег в начале аренды
        public double StartMileage { get; set; } 
        //Пробег в конце аренды
        public double? EndMileage { get; set; }

        public bool IsGPSNavigationSystem { get; set; } 
        public bool ChildSeat { get; set; } 
        public bool AdditionalDriver { get; set; } 
        public bool InsuranceCoverage { get; set; }

        public RentalHistoryStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }  
    }
}
