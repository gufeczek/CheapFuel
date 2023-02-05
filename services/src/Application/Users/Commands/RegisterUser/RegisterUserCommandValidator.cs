using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private const string AllowedSpecialCharactersForUsername = "._-";
    private const string AllowedCharactersForUsernameRegex = @"^[a-zA-Z0-9._-]+$";

    private const string LettersAndDigitsRegex = "[a-zA-Z0-9]";

    public RegisterUserCommandValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(32)
            .Must(username => 
                username is not null && 
                Regex.Matches(username, LettersAndDigitsRegex).Count >= 2)
            .WithMessage("Username must contains at least two letters or numbers")
            .Must(username => 
                username is not null && 
                Regex.IsMatch(username, AllowedCharactersForUsernameRegex))
            .WithMessage($"Username can contain only latin letters, numbers and {AllowedSpecialCharactersForUsername}");
        
        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
        
        RuleFor(r => r.Password)
            .SetValidator(new PasswordValidator())
            .Must((model, password) => 
                password is not null &&
                password.Equals(model.ConfirmPassword))
            .WithMessage("Your passwords do not match");
    }
}