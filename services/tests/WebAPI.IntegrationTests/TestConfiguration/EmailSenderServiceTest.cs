using System.Threading.Tasks;
using Application.Common.Interfaces;

namespace WebAPI.IntegrationTests.TestConfiguration;

public class EmailSenderServiceTest : IEmailSenderService
{
    public Task SendEmailAddressVerificationToken(string destEmail, string token)
    {
        return Task.CompletedTask;
    }

    public Task SendPasswordResetToken(string destEmail, string token)
    {
        return Task.CompletedTask;
    }
}