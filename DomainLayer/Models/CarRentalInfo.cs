using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class CarRentalInfo
    {
        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Image { get; set; }
        public int RentalsCount { get; set; }
        public int TotalDaysRented { get; set; }
        public decimal TotalEarnings { get; set; }
        public DateTime? FirstRentalDate { get; set; }
        public DateTime? LastRentalDate { get; set; }
    }
}
