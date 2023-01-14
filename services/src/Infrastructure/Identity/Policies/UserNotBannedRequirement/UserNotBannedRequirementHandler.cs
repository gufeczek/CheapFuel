using System.Security.Claims;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Identity.Policies.UserNotBannedRequirement;

public class UserNotBannedRequirementHandler : AuthorizationHandler<UserNotBannedRequirement>
{
    private readonly IUserRepository _userRepository;

    public UserNotBannedRequirementHandler(IUnitOfWork unitOfWork)
    {
        _userRepository = unitOfWork.Users;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserNotBannedRequirement requirement)
    {
        var username = context
            .User
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;
        
        if (username is not null)
        {
        
            var isUserBanned = await _userRepository.IsUserBanned(username);
        
            if (!isUserBanned)
            {
                context.Succeed(requirement);
            }
        }
    }
}