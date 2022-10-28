using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.StationChains.Commands.CreateStationChain;

public sealed class CreateStationChainCommandHandler : IRequestHandler<CreateStationChainCommand, StationChainDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public CreateStationChainCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<StationChainDto> Handle(CreateStationChainCommand request, CancellationToken cancellationToken)
    {
        var stationChain = new StationChain
        {
            Name = request.Name
        };
        
        _unitOfWork.StationChains.Add(stationChain);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<StationChainDto>(stationChain);
    }
}