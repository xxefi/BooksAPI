using Books.Core.Abstractions.Services;
using Books.Core.Dtos.Update;
using FluentValidation;
using static Books.Core.Constants.ValidationConstants;

namespace Books.Application.Validators.Update;

public class UpdateReviewValidator : AbstractValidator<UpdateReviewDto>
{
    public UpdateReviewValidator(ILocalizationService ls)
    {
        RuleFor(r => r.Content)
            .MaximumLength(MaxReviewContentLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("ReviewTextMaxLength"), MaxReviewContentLength))
            .Matches(ReviewContentRegex)
            .WithMessage(_ => ls.GetLocalizedString("ReviewTextInvalidCharacters"))
            .When(r => r.Content != null);

        RuleFor(r => r.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage(_ => ls.GetLocalizedString("RatingRange"))
            .When(r => r.Rating.HasValue);
    }
}