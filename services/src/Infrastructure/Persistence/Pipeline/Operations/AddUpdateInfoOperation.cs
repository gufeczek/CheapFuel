using Application.Common.Authentication;
using Domain.Common.Interfaces;
using Infrastructure.Persistence.Pipeline.Operations.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence.Pipeline.Operations;

public class AddUpdateInfoOperation : IAddUpdateInfoOperation
{
    private readonly IUserPrincipalService _userPrincipalIdentityService;

    public AddUpdateInfoOperation(IUserPrincipalService userPrincipalIdentityService)
    {
        _userPrincipalIdentityService = userPrincipalIdentityService;
    }

    public void Invoke(EntityEntry entity)
    {
        if (entity.Entity is not IUpdatable updatable
            || entity.State is not (EntityState.Added or EntityState.Modified))
        {
            return;
        }
        
        updatable.UpdatedAt = DateTime.UtcNow;
        updatable.UpdatedBy = _userPrincipalIdentityService.GetUserPrincipalId();
    }
}