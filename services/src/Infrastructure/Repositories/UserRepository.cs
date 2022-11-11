using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }
    
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await Context.Users
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmailAddressAsync(string email)
    {
        return await Context.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByUsername(string username)
    {
        return await Context.Users
            .AnyAsync(u => u.Username == username);
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        return await Context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<bool?> IsEmailVerified(string username)
    {
        return await Context.Users
            .Where(u => u.Username == username)
            .Select(u => u.EmailConfirmed)
            .FirstAsync();
    }
}