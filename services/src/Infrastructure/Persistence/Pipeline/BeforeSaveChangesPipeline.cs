using Infrastructure.Persistence.Pipeline.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence.Pipeline;

public class BeforeSaveChangesPipeline : IPipeline<EntityEntry>
{
    private readonly List<IOperation<EntityEntry>> _operations = new();

    public void AddOperation(IOperation<EntityEntry> operation)
    {
        _operations.Add(operation);
    }
    
    public void Invoke(IEnumerable<EntityEntry> data)
    {
        var entities = data.ToList();
        
        foreach (var operation in _operations)
        {
            foreach (var entity in entities)
            {
                operation.Invoke(entity);
            }
        }
    }
}