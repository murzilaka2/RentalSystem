using DomainLayer.Models;
using RentalSystem.Models;

namespace DomainLayer.Interfaces
{
    public interface IRental
    {
        Task<bool> RentCarAsync(Rental rental);
        Task<(IEnumerable<Rental> Rentals, int TotalCount)> GetRentalsAsync(PaginationModel pagination);
        Task<(IEnumerable<Rental> Rentals, int TotalCount)> GetRentalsCustomerAsync(int customerId, PaginationModel pagination);
        Task<bool> RemoveRentalAsync(int rentalId);
        Task<Rental> GetRentalAsync(int rentalId);
        Task<bool> UpdateRentalAsync(Rental rental);
        Task<bool> CancelRentalAsync(int rentalId);
        Task<bool> CloseRentalAsync(int rentalId);
    }
}
