using System.Security.Claims;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity;

public class UserPrincipalService : IUserPrincipalService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserPrincipalService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserName()
    {
        return _httpContextAccessor
            .HttpContext?
            .User
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;
    }

    public int? GetUserPrincipalId()
    {
        var claim = _httpContextAccessor
            .HttpContext?
            .User
            .FindFirst(claim => claim.Type.Equals("Id"));
        return claim != null ? int.Parse(claim.Value) : null;
    }
}