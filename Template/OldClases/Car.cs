namespace RentalSystem.Models
{
    public abstract class Car
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal RentalPrice { get; set; }
        public CarStatus Status { get; set; }

        public Rental CurrentRental { get; set; }
        protected Car()
        {
            
        }
        public Car(string brand, string model, int year, decimal rentalPrice)
        {
            Brand = brand;
            Model = model;
            Year = year;
            RentalPrice = rentalPrice;
            Status = CarStatus.Available;
        }
        public double CalculateRentalPrice()
        {
            return (double)RentalPrice;
        }
        public abstract void DisplayInfo();

        public void RentCar()
        {
            Status = CarStatus.Rented;
        }

        public void ReturnCar()
        {
            Status = CarStatus.Available;
        }
    }
}
