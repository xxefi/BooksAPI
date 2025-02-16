using Books.Core.Abstractions.Services;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Update;
using FluentValidation;

namespace Books.Application.Validators.Update;

public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemDto>
{
    public UpdateOrderItemValidator(ILocalizationService ls)
    {
        RuleFor(r => r.Quantity)
            .GreaterThan(0)
            .WithMessage(_ => ls.GetLocalizedString("QuantityGreaterThanZero"))
            .When(r => r.Quantity.HasValue);

        RuleFor(r => r.Price)
            .GreaterThan(0)
            .WithMessage(_ => ls.GetLocalizedString("PriceGreaterThanZero"))
            .When(r => r.Price.HasValue);
    }
}