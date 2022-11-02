using FluentValidation;

namespace Application.Users.Commands.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(r => r.OldPassword)
            .NotEmpty();
        
        RuleFor(r => r.NewPassword)
            .SetValidator(new PasswordValidator())
            .Must((model, password) => 
                password is not null &&
                password.Equals(model.ConfirmNewPassword))
            .WithMessage("Your passwords do not match");
    }
}