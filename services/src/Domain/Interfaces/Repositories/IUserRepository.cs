using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUserRepository: IBaseRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);

    Task<bool> ExistsByUsername(string username);
    Task<bool> ExistsByEmail(string email);
}