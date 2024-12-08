using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Refunded, // Возвращены средства
        Cancelled
    }
}
