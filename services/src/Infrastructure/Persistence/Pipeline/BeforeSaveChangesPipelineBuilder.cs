using Infrastructure.Persistence.Pipeline.Operations.Interfaces;

namespace Infrastructure.Persistence.Pipeline;

public class BeforeSaveChangesPipelineBuilder : IBeforeSaveChangesPipelineBuilder
{
    private readonly IAddCreationInfoOperation _addCreationInfoOperation;
    private readonly IAddUpdateInfoOperation _addUpdateInfoOperation;
    private readonly IRemovalHandlingOperation _removalHandlingOperation;

    public BeforeSaveChangesPipelineBuilder(IAddCreationInfoOperation addCreationInfoOperation, IAddUpdateInfoOperation addUpdateInfoOperation, IRemovalHandlingOperation removalHandlingOperation)
    {
        _addCreationInfoOperation = addCreationInfoOperation;
        _addUpdateInfoOperation = addUpdateInfoOperation;
        _removalHandlingOperation = removalHandlingOperation;
    }

    public BeforeSaveChangesPipeline Build()
    {
        var pipeline = new BeforeSaveChangesPipeline();
        pipeline.AddOperation(_addCreationInfoOperation);
        pipeline.AddOperation(_addUpdateInfoOperation);
        pipeline.AddOperation(_removalHandlingOperation);
        
        return pipeline;
    }
}