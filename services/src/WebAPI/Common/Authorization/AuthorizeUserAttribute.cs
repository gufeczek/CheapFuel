using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Common.Authorization;

public class AuthorizeUserAttribute : AuthorizeAttribute
{
    public AuthorizeUserAttribute()
    {
        Policy = "EmailVerified";
        Roles = string.Join(",", Role.User, Role.Owner, Role.Admin);
    }
}