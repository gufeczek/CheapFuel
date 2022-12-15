using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.ServiceAtStations.Commands.AddServiceToStation;

public sealed class AddServiceToStationCommandHandler : IRequestHandler<AddServiceToStationCommand, ServiceAtStationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelStationServiceRepository _fuelStationServiceRepository;
    private readonly IServiceAtStationRepository _serviceAtStationRepository;
    private readonly IMapper _mapper;

    public AddServiceToStationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelStationServiceRepository = unitOfWork.Services;
        _serviceAtStationRepository = unitOfWork.ServicesAtStation;
        _mapper = mapper;
    }
    
    public async Task<ServiceAtStationDto> Handle(AddServiceToStationCommand request, CancellationToken cancellationToken)
    {
        if (await _serviceAtStationRepository.ExistsAsync(request.FuelStationId, request.ServiceId))
        {
            throw new ConflictException($"Fuel station with id = {request.FuelStationId} already has service with id = {request.ServiceId}");
        }
        
        var fuelStation = await _fuelStationRepository.GetAsync(request.FuelStationId)
                          ?? throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");
        var service = await _fuelStationServiceRepository.GetAsync(request.ServiceId)
                       ?? throw new NotFoundException($"Service not found for id = {request.ServiceId}");

        var serviceAtStation = new ServiceAtStation
        {
            FuelStation = fuelStation,
            Service = service
        };
        
        _serviceAtStationRepository.Add(serviceAtStation);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ServiceAtStationDto>(serviceAtStation);
    }
}