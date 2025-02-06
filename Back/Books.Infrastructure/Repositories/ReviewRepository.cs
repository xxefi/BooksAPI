using System.Linq.Expressions;
using Books.Core.Abstractions.Repositories;
using Books.Core.Models;
using Books.Infrastructure.Context;

namespace Books.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly BooksContext _context;

    public ReviewRepository(BooksContext context)
        => _context = context;


    public async Task<Review> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(Review review)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(IEnumerable<Review> reviews)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Review>> FindAsync(Expression<Func<Review, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}