using DomainLayer.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalSystem.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public double EngineDisplacement { get; set; }
        public int Year { get; set; }
        public CarType CarType { get; set; }
        public Transmission Transmission { get; set; }
        public double CurrentMileage { get; set; }

        //Лимит миль на аренду. Если -1, то без ограничения
        public int MileageLimit { get; set; }
        public int SeatsCount { get; set; }

        //Цена в сутки
        public decimal Price { get; set; }
        
        //Статус аренды (используем только при работе с арендой)
        public RentalStatus RentalStatus { get; set; }
        public int DealerId { get; set; }
        public Dealer Dealer { get; set; }

        [NotMapped]
        public double AverageRating { get; set; }
        [NotMapped]
        public int ReviewCount { get; set; }
    }
}
