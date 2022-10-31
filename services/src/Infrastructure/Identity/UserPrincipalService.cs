using Application.Common;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity;

public interface IUserPrincipalService
{
    int GetUserPrincipalId();
}

public class UserPrincipalService : IUserPrincipalService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserPrincipalService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetUserPrincipalId()
    {
        var claim = _httpContextAccessor
            .HttpContext?
            .User
            .FindFirst(claim => claim.Type.Equals("Id"))
            .OrElseThrow("Id claim of user principal is equal to null. User is probably not logged in!");
        return int.Parse(claim!.Value);
    }
}