namespace Infrastructure.Persistence.Pipeline.Common;

public interface IOperation<TInput>
{
    void Invoke(TInput data);
}