using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public enum RentalStatus
    {
        Active = 0, //Готова к аренде
        InRent, //Находится в аренде
        UnderRepair, //Находится в ремонте
        Reserved //Забронирована
    }
}
