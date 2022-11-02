using Domain.Entities.Tokens;

namespace Domain.Interfaces.Repositories.Tokens;

public interface IEmailVerificationTokenRepository : ITokenRepository<EmailVerificationToken>
{

}