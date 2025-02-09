using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace Books.Presentation.Controllers.Main;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
        => _reviewService = reviewService;

    [HttpGet("GetReviews")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _reviewService.GetAllReviewsAsync());

    [HttpGet("ID/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _reviewService.GetReviewByIdAsync(id) ?? throw new BookException(ExceptionType.NotFound, "ReviewNotFound"));

    [HttpGet("book/{bookId:guid}")]
    public async Task<IActionResult> GetByBookId(Guid bookId) =>
        Ok(await _reviewService.GetReviewsByBookIdAsync(bookId));

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId) =>
        Ok(await _reviewService.GetReviewsByUserIdAsync(userId));

    [HttpGet("GetRecentReviews")]
    public async Task<IActionResult> GetRecentReviews([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) =>
        Ok(await _reviewService.GetRecentReviewsAsync(pageNumber, pageSize));

    [HttpGet("averageRating/{bookId:guid}")]
    public async Task<IActionResult> GetAverageRating(Guid bookId) =>
        Ok(await _reviewService.GetAverageRatingByBookIdAsync(bookId));

    [HttpGet("GetTotalReviewsCount")]
    public async Task<IActionResult> GetTotalReviewsCount() =>
        Ok(await _reviewService.GetTotalReviewsCountAsync());

    [HttpGet("GetReviewsCountByBook/{bookId:guid}")]
    public async Task<IActionResult> GetReviewsCountByBookId(Guid bookId) =>
        Ok(await _reviewService.GetReviewsCountByBookIdAsync(bookId));

    [HttpGet("GetReviewsCountByUser/{userId:guid}")]
    public async Task<IActionResult> GetReviewsCountByUserId(Guid userId) =>
        Ok(await _reviewService.GetReviewsCountByUserIdAsync(userId));

    [HttpPost("CreateReview")]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto reviewDto) =>
        Ok(await _reviewService.CreateReviewAsync(reviewDto));

    [HttpPut("Update/ID/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReviewDto reviewDto) =>
        Ok(await _reviewService.UpdateReviewAsync(id, reviewDto));

    [HttpDelete("Delete/ID/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _reviewService.DeleteReviewAsync(id));
}