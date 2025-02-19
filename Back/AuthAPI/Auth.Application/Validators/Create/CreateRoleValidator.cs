using Auth.Core.Abstractions.Services;
using Auth.Core.Dtos.Create;
using FluentValidation;
using static Auth.Core.Constants.ValidationConstants;

namespace Auth.Application.Validators.Create;

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