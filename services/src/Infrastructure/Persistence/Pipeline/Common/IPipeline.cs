namespace Infrastructure.Persistence.Pipeline.Common;

public interface IPipeline<TInput> : IOperation<IEnumerable<TInput>>
{
    public void AddOperation(IOperation<TInput> operation);
}