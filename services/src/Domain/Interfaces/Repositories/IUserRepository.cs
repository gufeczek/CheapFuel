using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUserRepository: IBaseRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAddressAsync(string email);

    Task<bool> ExistsByUsername(string username);
    Task<bool> ExistsByEmail(string email);
    
    Task<bool?> IsEmailVerified(string username);
    
}