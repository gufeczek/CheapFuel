using Domain.Entities.Tokens;
using Domain.Interfaces.Repositories.Tokens;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories.Tokens;

public class EmailVerificationTokenRepository : TokenRepository<EmailVerificationToken>, IEmailVerificationTokenRepository
{
    public EmailVerificationTokenRepository(AppDbContext context) : base(context) { }
}