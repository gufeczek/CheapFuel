using Domain.Entities.Tokens;

namespace Domain.Interfaces.Repositories.Tokens;

public interface IPasswordResetTokenRepository : ITokenRepository<PasswordResetToken>
{
    
}