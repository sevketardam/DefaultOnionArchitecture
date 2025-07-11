using FluentValidation;

namespace DefaultOnionArchitecture.Application.Features.Auth.Command.Revoke;

public class RevokeCommandValidator : AbstractValidator<RevokeCommandRequest>
{
    public RevokeCommandValidator()
    {
        RuleFor(a => a.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
