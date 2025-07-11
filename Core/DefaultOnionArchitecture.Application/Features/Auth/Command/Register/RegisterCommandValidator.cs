using FluentValidation;

namespace DefaultOnionArchitecture.Application.Features.Auth.Command.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommandRequest>
{

    public RegisterCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MinimumLength(2)
            .WithName("İsim");

        RuleFor(x => x.Surname)
            .NotEmpty()
            .MaximumLength(50)
            .MinimumLength(2)
            .WithName("Soyisim");

        RuleFor(a => a.Email)
            .NotEmpty()
            .MaximumLength(60)
            .EmailAddress()
            .MinimumLength(8)
            .WithName("E-posta Adresi");

        RuleFor(a => a.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithName("Parola");

        RuleFor(a => a.ConfirmPassword)
            .NotEmpty()
            .MinimumLength(6)
            .Equal(x => x.Password)
            .WithName("Parola Tekrarı");
    }
}
