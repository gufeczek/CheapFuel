using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Identity.Policies.UserNotBannedRequirement;

public sealed class UserNotBannedRequirement : IAuthorizationRequirement { }