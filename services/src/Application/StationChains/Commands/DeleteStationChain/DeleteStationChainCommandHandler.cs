using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.StationChains.Commands.DeleteStationChain;

public class DeleteStationChainCommandHandler : IRequestHandler<DeleteStationChainCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStationChainRepository _stationChainRepository;

    public DeleteStationChainCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _stationChainRepository = unitOfWork.StationChains;
    }

    public async Task<Unit> Handle(DeleteStationChainCommand request, CancellationToken cancellationToken)
    {
        var stationChain = await _stationChainRepository.GetAsync(request.Id.OrElseThrow())
                           ?? throw new NotFoundException($"Station chain not found for if = {request.Id}");
        
        _stationChainRepository.Remove(stationChain);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}