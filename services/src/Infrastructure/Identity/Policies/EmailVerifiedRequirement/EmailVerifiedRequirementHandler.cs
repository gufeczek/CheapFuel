using System.Security.Claims;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Identity.Policies.EmailVerifiedRequirement;

public class EmailVerifiedRequirementHandler : AuthorizationHandler<EmailVerifiedRequirement>
{
    private readonly IUserRepository _userRepository;

    public EmailVerifiedRequirementHandler(IUnitOfWork unitOfWork)
    {
        _userRepository = unitOfWork.Users;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailVerifiedRequirement requirement)
    {
        var username = context
            .User
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;

        if (username is not null)
        {
            var isEmailVerified = await _userRepository.IsEmailVerified(username) ?? false;
        
            if (isEmailVerified)
            {
                context.Succeed(requirement);
            }
        }
    }
}