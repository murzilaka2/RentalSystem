﻿namespace RentalSystem.Models
{
    public class Sedan : Car
    {
        public Sedan(string brand, string model, int year, decimal rentalPrice)
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
