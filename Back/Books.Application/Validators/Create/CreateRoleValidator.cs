using Books.Core.Abstractions.Services;
using Books.Core.Dtos.Create;
using FluentValidation;
using static Books.Core.Constants.ValidationConstants;

namespace Books.Application.Validators.Create;

public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleValidator(ILocalizationService ls)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ls.GetLocalizedString("RoleNameRequired"))
            .MinimumLength(MinRoleNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("RoleNameMinLength"), MinRoleNameLength))
            .MaximumLength(MaxRoleNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("RoleNameMaxLength"), MaxRoleNameLength));
    }
}