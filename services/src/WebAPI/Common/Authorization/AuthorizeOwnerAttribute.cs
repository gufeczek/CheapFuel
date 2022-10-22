using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Common.Authorization;

public class AuthorizeOwnerAttribute : AuthorizeAttribute
{
    public AuthorizeOwnerAttribute()
    {
        Roles = string.Join(",", Role.Owner, Role.Admin);
    }
}