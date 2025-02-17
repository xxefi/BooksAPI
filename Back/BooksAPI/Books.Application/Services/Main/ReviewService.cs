using AutoMapper;
using Books.Application.Exceptions;
using Books.Application.Validators.Create;
using Books.Application.Validators.Update;
using Books.Core.Abstractions.Repositories;
using Books.Core.Abstractions.Repositories.Main;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Abstractions.UOW;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Entities;

namespace Books.Application.Services.Main;

public class ReviewService : IReviewService
{
    private readonly IMapper _mapper;
    private readonly CreateReviewValidator _createReviewValidator;
    private readonly UpdateReviewValidator _updateReviewValidator;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReviewService(IMapper mapper, CreateReviewValidator createReviewValidator,
        UpdateReviewValidator updateReviewValidator, IReviewRepository reviewRepository,
         IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _createReviewValidator = createReviewValidator;
        _updateReviewValidator = updateReviewValidator;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
    {
        var reviews = await _reviewRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<ReviewDto?> GetReviewByIdAsync(Guid id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        return _mapper.Map<ReviewDto>(review);
    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsByBookIdAsync(Guid bookId)
    {
        var reviews = await _reviewRepository.FindAsync(b => b.BookId == bookId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(Guid userId)
    {
        var reviews = await _reviewRepository.FindAsync(b => b.UserId == userId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<IEnumerable<ReviewDto>> GetRecentReviewsAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            throw new BookException(ExceptionType.BadRequest, "PaginationError");
        
        var reviews = await _reviewRepository.GetAllAsync();
        var sortedReviews = reviews
            .OrderByDescending(r => r.CreatedAt);
        
        var pagedViews = sortedReviews
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        
        return _mapper.Map<IEnumerable<ReviewDto>>(pagedViews);
    }

    public async Task<double> GetAverageRatingByBookIdAsync(Guid bookId)
    {
        var reviews = await _reviewRepository.FindAsync(b => b.BookId == bookId);
        return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
    }

    public async Task<int> GetTotalReviewsCountAsync()
    {
        var reviews = await _reviewRepository.GetAllAsync();
        return reviews.Count();
    }

    public async Task<int> GetReviewsCountByBookIdAsync(Guid bookId)
    {
        var reviews = await _reviewRepository.FindAsync(b => b.BookId == bookId);
        return reviews.Count;
    }

    public async Task<int> GetReviewsCountByUserIdAsync(Guid userId)
    {
        var reviews = await _reviewRepository.FindAsync(b => b.UserId == userId);
        return reviews.Any() ? reviews.Sum(r => r.Rating) : 0;
    }

    public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto reviewDto)
    {
        var validator = await _createReviewValidator.ValidateAsync(reviewDto);
        if (!validator.IsValid)
            throw new BookException(ExceptionType.InvalidRequest, 
                string.Join(", ", validator.Errors));
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var review = _mapper.Map<Review>(reviewDto);
            await _reviewRepository.AddAsync(review);
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<ReviewDto>(review);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<ReviewDto> UpdateReviewAsync(Guid id, UpdateReviewDto reviewDto)
    {
        var existingReview = await _reviewRepository.GetByIdAsync(id);
        var validator = await _updateReviewValidator.ValidateAsync(reviewDto);
        if (!validator.IsValid)
            throw new BookException(ExceptionType.InvalidRequest, 
                string.Join(", ", validator.Errors));
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _mapper.Map(reviewDto, existingReview);
            await _reviewRepository.UpdateAsync(new[] { existingReview });
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<ReviewDto>(existingReview);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteReviewAsync(Guid id)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _reviewRepository.DeleteAsync(id);
            await _unitOfWork.CommitTransactionAsync();
            
            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}