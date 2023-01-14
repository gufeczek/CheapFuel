using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using Domain.Entities;
using Domain.Enums;
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

    public async Task<Page<User>> GetAllAsync(Role? role, AccountStatus? status, string? searchPhrase, PageRequest<User> pageRequest)
    {
        var query = Context.Users
            .Where(u =>
                (role == null || u.Role == role) &&
                (status == null || u.Status == status) && 
                (searchPhrase == null || u.Username!.ToLower().Contains(searchPhrase.ToLower())));
        return await Paginate(query, pageRequest);
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
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsUserBanned(string username)
    {
        return await Context.Users
            .Where(u => u.Username == username)
            .AnyAsync(u => u.Status == AccountStatus.Banned);
    }
}