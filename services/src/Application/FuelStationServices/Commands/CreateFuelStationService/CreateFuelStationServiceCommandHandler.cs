using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStationServices.Commands.CreateFuelStationService;

public sealed class CreateFuelStationServiceCommandHandler : IRequestHandler<CreateFuelStationServiceCommand, FuelStationServiceDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationServiceRepository _serviceRepository;
    private readonly IMapper _mapper;

    public CreateFuelStationServiceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _serviceRepository = unitOfWork.Services;
        _mapper = mapper;
    }
    
    public async Task<FuelStationServiceDto> Handle(CreateFuelStationServiceCommand request, CancellationToken cancellationToken)
    {
        var service = new FuelStationService
        {
            Name = request.Name
        };
        
        _serviceRepository.Add(service);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<FuelStationServiceDto>(service);
    }
}