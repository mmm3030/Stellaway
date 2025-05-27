using FluentValidation;
using Stellaway.Common.Resources;

namespace Stellaway.DTOs;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage(Resource.UsernameRequired);
        RuleFor(x => x.Password).NotEmpty().WithMessage(Resource.PasswordRequired);
    }
}