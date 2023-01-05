﻿using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BlockUserRepository : Repository<BlockedUser>, IBlockUserRepository
{
    public BlockUserRepository(AppDbContext context) : base(context) { }
    
    public async Task<bool> ExistsByBlockedUserId(long userId)
    {
        return await Context.ReportedReviews
            .Where(r => r.UserId == userId)
            .AnyAsync();
    }
    
    public async Task<BlockedUser?> GetByBlockedUserId(long userId)
    {
        return await Context.BlockedUsers
            .Where(r => r.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<BlockedUser?> DeleteExpiredBans()
    {
        return await Context.BlockedUsers
            .Where(r => r.EndBanDate < DateTime.Now)
            .FirstOrDefaultAsync();
    }
}