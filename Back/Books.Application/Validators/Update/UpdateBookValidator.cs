using FluentValidation;
using Books.Core.Dtos.Update;
using Books.Core.Abstractions.Services;
using static Books.Core.Constants.ValidationConstants;

namespace Books.Application.Validators.Update
{
    public class UpdateBookValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookValidator(ILocalizationService ls)
        {
            RuleFor(b => b.Title)
                .NotEmpty()
                .When(b => !string.IsNullOrEmpty(b.Title))
                .WithMessage(_ => ls.GetLocalizedString("TitleRequired"))
                .Matches(TitleRegex)
                .WithMessage(_ => ls.GetLocalizedString("TitleInvalid"))
                .MaximumLength(MaxTitleLength)
                .WithMessage(_ => string.Format(ls.GetLocalizedString("TitleMaxLength"), MaxTitleLength));

            RuleFor(b => b.Author)
                .NotEmpty()
                .When(b => !string.IsNullOrEmpty(b.Author))
                .WithMessage(_ => ls.GetLocalizedString("AuthorRequired"))
                .Matches(AuthorRegex)
                .WithMessage(_ => ls.GetLocalizedString("AuthorInvalid"))
                .MaximumLength(MaxAuthorLength)
                .WithMessage(_ => string.Format(ls.GetLocalizedString("AuthorMaxLength"), MaxAuthorLength));

            RuleFor(b => b.Year)
                .Must(year => year.HasValue && year.Value > 0)
                .When(b => b.Year.HasValue)
                .WithMessage(_ => ls.GetLocalizedString("YearInvalid"));

            RuleFor(b => b.Genre)
                .NotEmpty()
                .When(b => !string.IsNullOrEmpty(b.Genre))
                .WithMessage(_ => ls.GetLocalizedString("GenreRequired"))
                .Matches(GenreRegex)
                .WithMessage(_ => ls.GetLocalizedString("GenreInvalid"))
                .MaximumLength(MaxGenreLength)
                .WithMessage(_ => string.Format(ls.GetLocalizedString("GenreMaxLength"), MaxGenreLength));
        }
    }
}