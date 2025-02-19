using Auth.Core.Abstractions.Services;
using Auth.Core.Dtos.Create;
using FluentValidation;
using static Auth.Core.Constants.ValidationConstants;

namespace Auth.Application.Validators.Create;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator(ILocalizationService ls)
    {
        RuleFor(u => u.Username)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("UsernameIsRequired"))
            .MinimumLength(MinUsernameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("UsernameMinLength"), MinUsernameLength))
            .MaximumLength(MaxUsernameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("UsernameMaxLength"), MaxUsernameLength))
            .Matches(UsernameRegex)
            .WithMessage(_ => ls.GetLocalizedString("UsernameInvalidFormat"));
        
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("FirstNameRequired"))
            .MinimumLength(MinNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("FirstNameMinLength"), MinNameLength))
            .MaximumLength(MaxNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("FirstNameMaxLength"), MaxNameLength))
            .Matches(NameRegex)
            .WithMessage(_ => ls.GetLocalizedString("FirstNameInvalidFormat"));
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("LastNameRequired"))
            .MinimumLength(MinNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("LastNameMinLength"), MinNameLength))
            .MaximumLength(MaxNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("LastNameMaxLength"), MaxNameLength))
            .Matches(NameRegex)
            .WithMessage(_ => ls.GetLocalizedString("LastNameInvalidFormat"));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("EmailRequired"))
            .EmailAddress()
            .WithMessage(_ => ls.GetLocalizedString("EmailInvalid"));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("PasswordRequired"))
            .MinimumLength(MinPasswordLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("PasswordMinLength"), MinPasswordLength))
            .MaximumLength(100)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("PasswordMaxLength"), MaxPasswordLength))
            .Matches(PasswordRegex)
            .WithMessage(_ => ls.GetLocalizedString("PasswordInvalidFormat"));

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage(_ => ls.GetLocalizedString("RoleRequired"));
    }
}