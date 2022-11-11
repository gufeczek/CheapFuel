using Domain.Entities.Tokens;
using Domain.Interfaces.Repositories.Tokens;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Tokens;

public class TokenRepository<T> : BaseRepository<T>, ITokenRepository<T> where T : AbstractToken
{
    public TokenRepository(AppDbContext context) : base(context) { }

    public async Task<T?> GetUserToken(string username)
    {
        return await Context.Set<T>()
            .Include(e => e.User)
            .Where(e => e.User!.Username == username)
            .FirstOrDefaultAsync();
    }

    public async Task RemoveAllByUsername(string username)
    {
        var tokens = await Context.Set<T>()
            .Include(e => e.User)
            .Where(e => e.User!.Username == username)
            .ToListAsync();
        Context.Set<T>().RemoveRange(tokens);
    }
}