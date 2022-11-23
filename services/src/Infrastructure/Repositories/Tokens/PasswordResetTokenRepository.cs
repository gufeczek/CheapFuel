using Domain.Entities.Tokens;
using Domain.Interfaces.Repositories.Tokens;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories.Tokens;

public class PasswordResetTokenRepository : TokenRepository<PasswordResetToken>, IPasswordResetTokenRepository
{
    public PasswordResetTokenRepository(AppDbContext context) : base(context) { }
}