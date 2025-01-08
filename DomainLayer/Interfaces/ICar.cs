using DomainLayer.Models;
using RentalSystem.Models;

namespace RentalSystem.Interfaces
{
    public interface ICar
    {
        Task<bool> AddCarAsync(Car car);
        Task<IEnumerable<string>> GetCarBrandAsync(bool isAll = true);
        Task<(IEnumerable<Car> Cars, int TotalCount)> GetAllCarsAsync(FilterModel filterModel);
        Task<(IEnumerable<Car> Cars, int TotalCount)> GetAllClientCarsAsync(PaginationModel pagination);
        Task<bool> RemoveCarAsync(int carId);
        Task<Car> GetCarAsync(int carId);
        Task<bool> UpdateCarAsync(Car car);
        Task<(IEnumerable<CarRentalInfo> Cars, int TotalCount)> GetMostRentedCarsAsync(FilterModel filterModel);
    }
}
