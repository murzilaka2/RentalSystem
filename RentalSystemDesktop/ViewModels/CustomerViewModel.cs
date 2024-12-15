using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalSystemDesktop.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Passport { get; set; }
        public int DrivingExperience { get; set; }
        public string Description { get; set; }
    }


}
