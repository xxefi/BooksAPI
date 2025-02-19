using Auth.Core.Dtos.Update;
using FluentValidation;

namespace Auth.Application.Validators.Update;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        
    }
}