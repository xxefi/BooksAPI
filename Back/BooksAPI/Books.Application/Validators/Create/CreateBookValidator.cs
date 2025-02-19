using FluentValidation;
using Books.Core.Dtos.Create;
using Books.Core.Abstractions.Services;
using Books.Core.Abstractions.Services.Main;
using static Books.Core.Constants.ValidationConstants;

namespace Books.Application.Validators.Create;

public class CreateBookValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookValidator(ILocalizationService ls)
    {
        RuleFor(b => b.Title)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("TitleRequired"))
            .Matches(TitleRegex)
            .WithMessage(_ => ls.GetLocalizedString("TitleInvalid"))
            .MaximumLength(MaxTitleLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("TitleMaxLength"), MaxTitleLength));

        RuleFor(b => b.Author)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("AuthorRequired"))
            .Matches(AuthorRegex)
            .WithMessage(_ => ls.GetLocalizedString("AuthorInvalid"))
            .MaximumLength(MaxAuthorLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("AuthorMaxLength"), MaxAuthorLength));

        RuleFor(b => b.Year)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("YearRequired"))
            .Must(year => year > 0)
            .WithMessage(_ => ls.GetLocalizedString("YearInvalid"));

        RuleFor(b => b.Genre)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("GenreRequired"))
            .Matches(GenreRegex)
            .WithMessage(_ => ls.GetLocalizedString("GenreInvalid"))
            .MaximumLength(MaxGenreLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("GenreMaxLength"), MaxGenreLength));

        RuleFor(b => b.Price)
            .GreaterThan(0)
            .WithMessage(_ => ls.GetLocalizedString("PriceInvalid"));

        RuleFor(b => b.ISBN)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("ISBNRequired"))
            .Matches(ISBNRegex)
            .WithMessage(_ => ls.GetLocalizedString("ISBNInvalid"));

        RuleFor(b => b.Description)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("DescriptionRequired"))
            .MaximumLength(MaxDescriptionLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("DescriptionMaxLength"), MaxDescriptionLength));
    }
}