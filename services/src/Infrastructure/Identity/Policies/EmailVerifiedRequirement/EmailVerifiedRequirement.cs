using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Identity.Policies.EmailVerifiedRequirement;

public sealed class EmailVerifiedRequirement : IAuthorizationRequirement { }