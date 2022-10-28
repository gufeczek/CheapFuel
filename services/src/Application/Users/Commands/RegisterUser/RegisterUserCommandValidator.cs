using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private const string AllowedSpecialCharactersForUsername = "._-";
    private const string AllowedSpecialCharactersForPassword = @"!#$%&'()*+,-.:;<=>?@[]^_`{}|~";

    private const string AllowedCharactersForUsernameRegex = @"^[a-zA-Z0-9._-]+$";
    private const string AllowedCharactersForPasswordRegex = @"^[a-zA-Z0-9!#$%&'()*+,-.:;<=>?@\[\]^_`{}|~]+$";

    private const string LettersAndDigitsRegex = "[a-zA-Z0-9]";
    private const string LowercaseLettersRegex = "[a-z]";
    private const string UppercaseLettersRegex = "[A-Z]";
    private const string DigitsRegex = "[0-9]";

    public RegisterUserCommandValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(32)
            .Must(username => Regex.Matches(username, LettersAndDigitsRegex).Count >= 2)
            .WithMessage("Username must contains at least two letters or numbers")
            .Must(username => Regex.IsMatch(username, AllowedCharactersForUsernameRegex))
            .WithMessage($"Username can contain only latin letters, numbers and {AllowedSpecialCharactersForUsername}");
        
        RuleFor(r => r.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(32)
            .Must(password => 
                Regex.IsMatch(password, LowercaseLettersRegex) &&
                Regex.IsMatch(password, UppercaseLettersRegex) && 
                Regex.IsMatch(password, DigitsRegex))
            .WithMessage("Password must contain at least one number and one uppercase and lowercase letter")
            .Must(password => Regex.IsMatch(password, AllowedCharactersForPasswordRegex))
            .WithMessage($"Password can contain only latin letters, numbers and {AllowedSpecialCharactersForPassword}")
            .Must((model, password) => password.Equals(model.ConfirmPassword))
            .WithMessage("Your passwords do not match");
    }
}