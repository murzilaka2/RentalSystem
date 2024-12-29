using DomainLayer.Models;

namespace DomainLayer.Interfaces
{
    public interface IDealer
    {
        Task<IEnumerable<Dealer>> GetDealersAsync();
        Task<(IEnumerable<Dealer> Dealers, int TotalCount)> GetAllDealersAsync(FilterModel filterModel);
        Task<(IEnumerable<Dealer> Dealers, int TotalCount)> GetAllDealersWithCarsCountAsync(FilterModel filterModel);
        Task<Dealer?> GetDealerAsync(int id);
        Task<bool> AddDealerAsync(Dealer dealer);
        Task<bool> UpdateDealerAsync(Dealer dealer);
        Task<bool> RemoveDealerAsync(int id);
    }
}
