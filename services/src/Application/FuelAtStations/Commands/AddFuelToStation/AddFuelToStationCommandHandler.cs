using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelAtStations.Commands.AddFuelToStation;

public sealed class AddFuelToStationCommandHandler : IRequestHandler<AddFuelToStationCommand, FuelAtStationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelTypeRepository _fuelFuelTypeRepository;
    private readonly IFuelAtStationRepository _fuelAtStationRepository;
    private readonly IMapper _mapper;

    public AddFuelToStationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelFuelTypeRepository = unitOfWork.FuelTypes;
        _fuelAtStationRepository = unitOfWork.FuelsAtStation;
        _mapper = mapper;
    }
    
    public async Task<FuelAtStationDto> Handle(AddFuelToStationCommand request, CancellationToken cancellationToken)
    {
        if (await _fuelAtStationRepository.Exists(request.FuelTypeId, request.FuelStationId))
        {
            throw new ConflictException($"Fuel station with id = {request.FuelStationId} already has fuel type with id = {request.FuelTypeId}");
        }
        
        var fuelStation = await _fuelStationRepository.GetAsync(request.FuelStationId)
                          ?? throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");
        var fuelType = await _fuelFuelTypeRepository.GetAsync(request.FuelTypeId)
                       ?? throw new NotFoundException($"Fuel type not found for id = {request.FuelTypeId}");

        var fuelAtStation = new FuelAtStation
        {
            FuelStation = fuelStation,
            FuelType = fuelType
        };
        
        _fuelAtStationRepository.Add(fuelAtStation);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<FuelAtStationDto>(fuelAtStation);
    }
}