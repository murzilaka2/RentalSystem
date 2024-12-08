using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Dealer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int WorkExperience { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string? WhatsApp { get; set; }
        public string? Fax { get; set; }
    }
}
