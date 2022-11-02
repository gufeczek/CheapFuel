using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmailVerificationTokenRepository : BaseRepository<EmailVerificationToken>, IEmailVerificationTokenRepository
{
    public EmailVerificationTokenRepository(AppDbContext context) : base(context) { }
    
    public async Task<EmailVerificationToken?> GetUserToken(string username)
    {
        return await Context.EmailVerificationTokens
            .Include(e => e.User)
            .Where(e => e.User!.Username == username)
            .FirstOrDefaultAsync();
    }

    public async Task RemoveAllByUsername(string username)
    {
        var tokens = await Context.EmailVerificationTokens
            .Include(e => e.User)
            .Where(e => e.User!.Username == username)
            .ToListAsync();
        Context.EmailVerificationTokens.RemoveRange(tokens);
    }
}