using DomainLayer.Models;
using RentalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces
{
    public interface IReview
    {
        Task<bool> AddReviewAsync(Review review);
        Task<(IEnumerable<Review> Reviews, int TotalCount, double AverageRating)> GetCarReviewsAsync(int carId, int page, int pageSize = 10);
        Task<(List<Review> Reviews, int TotalCount)> GetAllReviewsAsync(int page, int pageSize = 10);
        Task<(List<Review> Reviews, int TotalCount)> GetClientReviewsAsync(int userId, int page, int pageSize = 10);
        Task<bool> RemoveReviewAsync(int id);
    }
}
