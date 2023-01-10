using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.Repositories;

public interface IUserRepository: IBaseRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAddressAsync(string email);
    Task<Page<User>> GetAllAsync(Role? role, AccountStatus? status, string? searchPhrase, PageRequest<User> pageRequest);

    Task<bool> ExistsByUsername(string username);
    Task<bool> ExistsByEmail(string email);
    
    Task<bool?> IsEmailVerified(string username);
    Task<bool> IsUserBanned(string username);
}