namespace Application.Common.Interfaces;

public interface IEmailSenderService
{
    Task SendEmailAddressVerificationToken(string destEmail, string token);
}