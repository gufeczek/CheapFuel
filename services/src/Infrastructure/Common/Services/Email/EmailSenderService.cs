using System.Net;
using System.Net.Mail;
using Application.Common.Interfaces;

namespace Infrastructure.Common.Services.Email;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailHostSettings _emailHostSettings;

    public EmailSenderService(EmailHostSettings emailHostSettings)
    {
        _emailHostSettings = emailHostSettings;
    }

    public async Task SendEmailAddressVerificationToken(string destEmail, string token)
    {
        using var mailMessage = new MailMessage();
        mailMessage.To.Add(new MailAddress(destEmail));
        mailMessage.From = new MailAddress(_emailHostSettings.EmailAddress!);
        mailMessage.Subject = "Email verification";
        mailMessage.Body = token;
        mailMessage.IsBodyHtml = false;

        using var smtp = new SmtpClient();
        smtp.Host = _emailHostSettings.Host!;
        smtp.Port = (int)_emailHostSettings.Port!;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(_emailHostSettings.EmailAddress, _emailHostSettings.Password);
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(mailMessage);
    }
}