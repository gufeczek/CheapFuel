using Application.Common.Authentication;
using Domain.Common.Interfaces;
using Infrastructure.Persistence.Pipeline.Operations.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence.Pipeline.Operations;

public class RemovalHandlingOperation : IRemovalHandlingOperation
{
    private readonly IUserPrincipalService _userPrincipalIdentityService;

    public RemovalHandlingOperation(IUserPrincipalService userPrincipalIdentityService)
    {
        _userPrincipalIdentityService = userPrincipalIdentityService;
    }

    public void Invoke(EntityEntry entity)
    {
        if (entity.Entity is not ISoftlyDeletable permanentEntity 
            || entity.State is not EntityState.Deleted)
        {
            return;
        }
        
        permanentEntity.Deleted = true;
        permanentEntity.DeletedAt = DateTime.UtcNow;
        permanentEntity.DeletedBy = _userPrincipalIdentityService.GetUserPrincipalId();

        entity.State = EntityState.Modified;
    }
}