using System.Net;
using System.Net.Mail;
using Application.Common.Interfaces;

namespace Infrastructure.Common.Services.Email;

public class EmailSenderService : IEmailSenderService
{
    private const string VerificationCodeEmailTemplatePlaceholder = "#VERIFICATION_CODE#";
    
    private readonly EmailHostSettings _emailHostSettings;

    public EmailSenderService(EmailHostSettings emailHostSettings)
    {
        _emailHostSettings = emailHostSettings;
    }

    public async Task SendEmailAddressVerificationToken(string destEmail, string token)
    {
        var body = Properties.Resources.EmailVerificationTemplate
            .Replace(VerificationCodeEmailTemplatePlaceholder, token);
        await SendEmail(destEmail, "Email verification", body);
    }

    public async Task SendPasswordResetToken(string destEmail, string token)
    {
        var body = Properties.Resources.ResetPasswordTemplate
            .Replace(VerificationCodeEmailTemplatePlaceholder, token);
        await SendEmail(destEmail, "Reset your password", body);
    }

    private async Task SendEmail(string destEmail, string subject, string body)
    {
        using var mailMessage = new MailMessage();
        mailMessage.To.Add(new MailAddress(destEmail));
        mailMessage.From = new MailAddress(_emailHostSettings.EmailAddress!);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = true;

        using var smtp = new SmtpClient();
        smtp.Host = _emailHostSettings.Host!;
        smtp.Port = (int)_emailHostSettings.Port!;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(_emailHostSettings.EmailAddress, _emailHostSettings.Password);
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(mailMessage);
    }
}