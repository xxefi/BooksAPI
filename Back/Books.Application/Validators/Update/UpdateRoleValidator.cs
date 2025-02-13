using Books.Core.Abstractions.Services;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Update;
using FluentValidation;
using static Books.Core.Constants.ValidationConstants;

namespace Books.Application.Validators.Update;

public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
{
    public UpdateRoleValidator(ILocalizationService ls)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ls.GetLocalizedString("RoleNameRequired"))
            .MinimumLength(MinRoleNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("RoleNameLength"), MinRoleNameLength))
            .MaximumLength(MaxRoleNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("RoleNameMaxLength"), MaxRoleNameLength));
    }
}