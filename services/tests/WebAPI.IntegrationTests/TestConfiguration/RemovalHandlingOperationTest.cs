using Infrastructure.Persistence.Pipeline.Operations.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WebAPI.IntegrationTests.TestConfiguration;

public class RemovalHandlingOperationTest : IRemovalHandlingOperation
{
    public void Invoke(EntityEntry data)
    {
        
    }
}