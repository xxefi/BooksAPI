using Books.Core.Abstractions.Services;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Update;
using FluentValidation;

namespace Books.Application.Validators.Update;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderDto>
{
    public UpdateOrderValidator(ILocalizationService ls)
    {
        RuleFor(o => o.OrderItems)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("OrderItemsRequired"))
            .Must(items => items.All(item => item.Quantity > 0 && item.Price > 0))
            .WithMessage(_ => ls.GetLocalizedString("OrderItemInvalid"));

        RuleFor(o => o.TotalPrice)
            .GreaterThan(0)
            .When(o => o.TotalPrice.HasValue)
            .WithMessage(_ => ls.GetLocalizedString("TotalPriceGreaterThanZero"));

        RuleFor(o => o.StatusId)
            .GreaterThan(0)
            .When(o => o.StatusId.HasValue)
            .WithMessage(_ => ls.GetLocalizedString("StatusIdRequired"));

        RuleFor(o => o.Address)
            .NotEmpty()
            .When(o => !string.IsNullOrEmpty(o.Address))
            .WithMessage(_ => ls.GetLocalizedString("AddressRequired"));
    }
}