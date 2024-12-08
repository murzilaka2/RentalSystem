namespace RentalSystem.Models
{
    public class Truck : Car
    {
        public Truck()
        {
            
        }
        public Truck(string brand, string model, int year, decimal rentalPrice)
            : base(brand, model, year, rentalPrice) { }

        public override void DisplayInfo()
        {
            

        }
        public double CalculateRentalPrice()
        {
            return (double)RentalPrice;
        }
    }
}
