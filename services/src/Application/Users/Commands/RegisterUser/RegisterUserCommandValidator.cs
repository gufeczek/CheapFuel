using FluentValidation;

namespace Application.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(32);

        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(r => r.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(32)
            .Must(password => 
                password.Any(char.IsLower) &&
                password.Any(char.IsUpper) && 
                password.Any(char.IsDigit))
            .WithMessage("Password must contain at least one number and one uppercase and lowercase letter")
            .Must((model, password) => password.Equals(model.ConfirmPassword))
            .WithMessage("Your passwords do not match");
    }
}