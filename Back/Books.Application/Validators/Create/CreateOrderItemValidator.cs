using Books.Core.Abstractions.Services;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Create;
using FluentValidation;

namespace Books.Application.Validators.Create;

public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemValidator(ILocalizationService ls)
    {
        RuleFor(r => r.BookId)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("BookIdRequired"));

        RuleFor(r => r.Quantity)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("QuantityRequired"))
            .GreaterThan(0)
            .WithMessage(_ => ls.GetLocalizedString("QuantityGreaterThanZero"));

        RuleFor(r => r.Price)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("PriceRequired"))
            .GreaterThan(0)
            .WithMessage(_ => ls.GetLocalizedString("PriceGreaterThanZero"));
    }
}