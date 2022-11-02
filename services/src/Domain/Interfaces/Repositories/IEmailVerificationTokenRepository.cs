using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IEmailVerificationTokenRepository : IBaseRepository<EmailVerificationToken>
{
    public Task<EmailVerificationToken?> GetUserToken(string username);
    public Task RemoveAllByUsername(string username);
}