using FluentValidation;

namespace Application.Users.Commands.ResetPassword;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(r => r.Token)
            .NotEmpty()
            .Length(6);

        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(r => r.NewPassword)
            .SetValidator(new PasswordValidator())
            .Must((model, password) =>
                password is not null &&
                password.Equals(model.ConfirmNewPassword))
            .WithMessage("Your passwords do not match")
            .WithName(nameof(ResetPasswordCommand.NewPassword));
    }
}