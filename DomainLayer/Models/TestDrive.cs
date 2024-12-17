using RentalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class TestDrive
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime Date { get; set; }
        public TestDriveStatus TestDriveStatus { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
