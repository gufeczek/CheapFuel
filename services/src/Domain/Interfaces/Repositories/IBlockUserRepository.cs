using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IBlockUserRepository : IRepository<BlockedUser>
{
    Task<bool> ExistsByBlockedUserId(long userId);

    Task<BlockedUser?> GetByBlockedUserId(long userId);

    Task RemoveAllExpiredBanned();
}