using System.Linq.Expressions;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Repositories.Main;
using Books.Core.Models;
using Books.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Repositories.Main;

public class ReviewRepository : IReviewRepository
{
    private readonly BooksContext _context;

    public ReviewRepository(BooksContext context)
        => _context = context;


    public async Task<Review> GetByIdAsync(Guid id)
        => await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Book)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id)
            ?? throw new BookException(ExceptionType.NotFound, "ReviewNotFound");

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Book)
            .AsNoTracking()
            .ToListAsync();

        return reviews.Any() ? reviews : throw new BookException(ExceptionType.NotFound, "NoReviewsFound");
    }

    public async Task AddAsync(Review review)
        => await _context.Reviews.AddAsync(review);

    public async Task UpdateAsync(IEnumerable<Review> reviews)
    {
        foreach (var review in reviews)
        {
            var updatedCount = await _context.Reviews
                .Where(r => r.Id == review.Id)
                .ExecuteUpdateAsync(r => r
                    .SetProperty(r => r.Content, review.Content)
                    .SetProperty(r => r.Rating, review.Rating)
                    .SetProperty(r => r.BookId, review.BookId)
                    .SetProperty(r => r.UserId, review.UserId));

            if (updatedCount == 0) throw new BookException(ExceptionType.NotFound, "ReviewNotFound");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var deletedCount = await _context.Reviews
            .Where(r => r.Id == id)
            .ExecuteDeleteAsync();

        if (deletedCount == 0) throw new BookException(ExceptionType.NotFound, "ReviewNotFound");
    }

    public async Task<ICollection<Review>> FindAsync(Expression<Func<Review, bool>> predicate)
        => await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Book)
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
}