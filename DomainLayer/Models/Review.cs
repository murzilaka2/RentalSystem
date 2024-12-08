using RentalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Review
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }  

        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
