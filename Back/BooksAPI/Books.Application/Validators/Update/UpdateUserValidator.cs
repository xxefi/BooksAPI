using Books.Core.Abstractions.Services;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Update;
using FluentValidation;
using static Books.Core.Constants.ValidationConstants;

namespace Books.Application.Validators.Update;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator(ILocalizationService ls)
    {
        RuleFor(u => u.Username)
            .NotEmpty()
            .WithMessage(ls.GetLocalizedString("UsernameIsRequired"))
            .MinimumLength(MinUsernameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("UsernameLength"), MinUsernameLength))
            .Matches(UsernameRegex)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("UsernameInvalidFormat"), UsernameRegex));

        RuleFor(u => u.FirstName)
            .NotEmpty()
            .WithMessage(ls.GetLocalizedString("FirstnameIsRequired"))
            .MinimumLength(MinNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("FirstNameMinLength"), MinNameLength))
            .MaximumLength(MaxNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("FirstNameMaxLength"), MaxNameLength))
            .Matches(NameRegex)
            .WithMessage(_ => ls.GetLocalizedString("FirstNameInvalidFormat"));
        
        RuleFor(u => u.LastName)
            .NotEmpty()
            .WithMessage(ls.GetLocalizedString("LastNameIsRequired"))
            .MinimumLength(MinNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("LastNameMinLength"), MinNameLength))
            .MaximumLength(MaxNameLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("LastNameMaxLength"), MaxNameLength))
            .Matches(NameRegex)
            .WithMessage(_ => ls.GetLocalizedString("LastNameInvalidFormat"));
        
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage(ls.GetLocalizedString("EmailRequired"))
            .EmailAddress()
            .WithMessage(ls.GetLocalizedString("EmailInvalid"));
        
        RuleFor(x => x.Password)
            .MinimumLength(MinPasswordLength)
            .WithMessage(_ => string.Format(ls.GetLocalizedString("PasswordMinLength"), MinPasswordLength));
        
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage(ls.GetLocalizedString("RoleRequired"));
    }
}