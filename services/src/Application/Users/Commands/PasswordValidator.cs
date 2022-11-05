using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Users.Commands;

public class PasswordValidator : AbstractValidator<string>
{
    private const string AllowedCharactersForPasswordRegex = @"^[a-zA-Z0-9!#$%&'()*+,-.:;<=>?@\[\]^_`{}|~]+$";
    private const string AllowedSpecialCharactersForPassword = @"!#$%&'()*+,-.:;<=>?@[]^_`{}|~";

    private const string LowercaseLettersRegex = "[a-z]";
    private const string UppercaseLettersRegex = "[A-Z]";
    private const string DigitsRegex = "[0-9]";
    
    public PasswordValidator()
    {
        RuleFor(password => password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(32)
            .Must(password =>
                password is not null &&
                Regex.IsMatch(password, LowercaseLettersRegex) &&
                Regex.IsMatch(password, UppercaseLettersRegex) &&
                Regex.IsMatch(password, DigitsRegex))
            .WithMessage("Password must contain at least one number and one uppercase and lowercase letter")
            .Must(password =>
                password is not null &&
                Regex.IsMatch(password, AllowedCharactersForPasswordRegex))
            .WithMessage($"Password can contain only latin letters, numbers and {AllowedSpecialCharactersForPassword}");
    }
}