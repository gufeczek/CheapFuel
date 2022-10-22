using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Common.Authorization;

public class AuthorizeAdminAttribute : AuthorizeAttribute
{
    public AuthorizeAdminAttribute()
    {
        Roles = Role.Admin.ToString();
    }
}