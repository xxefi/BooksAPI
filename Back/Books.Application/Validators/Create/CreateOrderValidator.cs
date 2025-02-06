using Books.Core.Abstractions.Services;
using Books.Core.Dtos.Create;
using FluentValidation;

namespace Books.Application.Validators.Create;

public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderValidator(ILocalizationService ls)
    {
        RuleFor(o => o.UserId)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("UserIdRequired"));

        RuleFor(o => o.OrderItems)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("OrderItemsRequired"))
            .Must(items => items.All(item => item.Quantity > 0 && item.Price > 0))
            .WithMessage(_ => ls.GetLocalizedString("OrderItemInvalid"));

        RuleFor(o => o.TotalPrice)
            .GreaterThan(0)
            .WithMessage(_ => ls.GetLocalizedString("TotalPriceGreaterThanZero"));

        RuleFor(o => o.StatusId)
            .GreaterThan(0)
            .WithMessage(_ => ls.GetLocalizedString("StatusIdRequired"));

        RuleFor(o => o.Address)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("AddressRequired"));
    }
}