using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;

namespace Books.Core.Abstractions.Services.Main;

public interface IReviewService
{
    Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
    Task<ReviewDto?> GetReviewByIdAsync(Guid id);
    Task<IEnumerable<ReviewDto>> GetReviewsByBookIdAsync(Guid bookId);
    Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(Guid userId);
    Task<IEnumerable<ReviewDto>> GetRecentReviewsAsync(int pageNumber, int pageSize);
    Task<double> GetAverageRatingByBookIdAsync(Guid bookId); 
    Task<int> GetTotalReviewsCountAsync();
    Task<int> GetReviewsCountByBookIdAsync(Guid bookId);
    Task<int> GetReviewsCountByUserIdAsync(Guid userId);
    Task<ReviewDto> CreateReviewAsync(CreateReviewDto reviewDto);
    Task<ReviewDto> UpdateReviewAsync(Guid id, UpdateReviewDto reviewDto);
    Task<bool> DeleteReviewAsync(Guid id);
}