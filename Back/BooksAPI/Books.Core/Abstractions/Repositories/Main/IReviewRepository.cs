using System.Linq.Expressions;
using Books.Core.Entities;

namespace Books.Core.Abstractions.Repositories.Main;

public interface IReviewRepository
{
    Task<Review> GetByIdAsync(Guid id);
    Task<IEnumerable<Review>> GetAllAsync();
    Task AddAsync(Review review);
    Task UpdateAsync(IEnumerable<Review> reviews);
    Task DeleteAsync(Guid id);
    Task<ICollection<Review>> FindAsync(Expression<Func<Review, bool>> predicate);
}