using Infrastructure.Persistence.Pipeline.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence.Pipeline.Operations.Interfaces;

public interface IRemovalHandlingOperation : IOperation<EntityEntry>
{
    
}