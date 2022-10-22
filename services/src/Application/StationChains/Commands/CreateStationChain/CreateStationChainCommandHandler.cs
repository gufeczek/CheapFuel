using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.StationChains.Commands.CreateStationChain;

public class CreateStationChainCommandHandler : IRequestHandler<CreateStationChainCommand, StationChain>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateStationChainCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<StationChain> Handle(CreateStationChainCommand request, CancellationToken cancellationToken)
    {
        var stationChain = new StationChain
        {
            Name = request.Name
        };
        
        _unitOfWork.StationChains.Add(stationChain);
        await _unitOfWork.SaveAsync();

        return stationChain;
    }
}