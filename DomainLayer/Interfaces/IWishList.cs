using DomainLayer.Models;
using RentalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces
{
    public interface IWishList
    {
        Task<List<WishList>> GetWishesListWithCarsAsync(int userId);
        Task<bool> AddWishListAsync(WishList wishList);
        Task<bool> RemoveWishListAsync(WishList wishList);
        Task<bool> IsCarInWishListAsync(WishList wishList);
    }
}
