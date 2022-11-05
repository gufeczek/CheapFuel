using Application.Common.Authentication;
using Domain.Common.Interfaces;
using Infrastructure.Persistence.Pipeline.Operations.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence.Pipeline.Operations;

public class AddCreationInfoOperation : IAddCreationInfoOperation
{
    private readonly IUserPrincipalService _userPrincipalIdentityService;

    public AddCreationInfoOperation(IUserPrincipalService userPrincipalIdentityService)
    {
        _userPrincipalIdentityService = userPrincipalIdentityService;
    }

    public void Invoke(EntityEntry entity)
    {
        if (entity.Entity is not ICreatable creatable
            || entity.State is not EntityState.Added)
        {
            return;
        }
        
        creatable.CreatedAt = DateTime.UtcNow;
        creatable.CreatedBy = _userPrincipalIdentityService.GetUserPrincipalId();
    }
}