using Domain.Entities.Tokens;

namespace Domain.Interfaces.Repositories.Tokens;

public interface ITokenRepository<T> : IBaseRepository<T> where T : AbstractToken
{
    public Task<T?> GetUserToken(string username);
    public Task RemoveAllByUsername(string username);
}