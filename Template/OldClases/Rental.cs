namespace RentalSystem.Models
{
    public class Rental
    {
        public Car RentedCar { get; set; }
        //public Customer Renter { get; set; }
        public User User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public InsuranceType Insurance { get; set; }

        public decimal CalculateRentalCost()
        {
            int rentalDays = (EndDate - StartDate).Days;
            decimal cost = rentalDays * RentedCar.RentalPrice;
            return cost;
        }
    }
}
