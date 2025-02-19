using Auth.Core.Dtos.Update;
using FluentValidation;

namespace Auth.Application.Validators.Update;

public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
{
    public UpdateRoleValidator()
    {
        
    }
}