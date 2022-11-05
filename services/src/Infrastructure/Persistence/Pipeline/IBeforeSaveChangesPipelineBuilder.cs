namespace Infrastructure.Persistence.Pipeline;

public interface IBeforeSaveChangesPipelineBuilder
{
    public BeforeSaveChangesPipeline Build();
}