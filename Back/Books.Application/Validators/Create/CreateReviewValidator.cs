using Books.Core.Abstractions.Services;
using Books.Core.Dtos.Create;
using FluentValidation;
using static Books.Core.Constants.ValidationConstants;

namespace Books.Application.Validators.Create;

public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewValidator(ILocalizationService ls)
    {
        RuleFor(r => r.Content)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("ReviewTextRequired"))
            .MaximumLength(MaxReviewContentLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("ReviewTextMaxLength"), MaxReviewContentLength))
            .Matches(ReviewContentRegex)
            .WithMessage(_ => ls.GetLocalizedString("ReviewTextInvalidCharacters"));
        
        RuleFor(r => r.Rating)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("RatingRequired"))
            .InclusiveBetween(1, 5)
            .WithMessage(_ => ls.GetLocalizedString("RatingRange"));

        RuleFor(r => r.UserId)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("UserIdRequired"));

        RuleFor(r => r.BookId)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("BookIdRequired"));
    }
}