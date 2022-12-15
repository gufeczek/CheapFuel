using Application.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelAtStations.Commands.RemoveFuelFromStation;

public sealed class RemoveFuelFromStationCommandHandler : IRequestHandler<RemoveFuelFromStationCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IFuelAtStationRepository _fuelAtStationRepository;

    public RemoveFuelFromStationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelTypeRepository = unitOfWork.FuelTypes;
        _fuelAtStationRepository = unitOfWork.FuelsAtStation;
    }
    
    public async Task<Unit> Handle(RemoveFuelFromStationCommand request, CancellationToken cancellationToken)
    {
        if (!await _fuelStationRepository.ExistsById(request.FuelStationId))
        {
            throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");
        }

        if (!await _fuelTypeRepository.ExistsById(request.FuelTypeId))
        {
            throw new NotFoundException($"Fuel type not found for id = {request.FuelTypeId}");
        }

        var fuelAtStation = await _fuelAtStationRepository.GetAsync(request.FuelStationId, request.FuelTypeId)
                            ?? throw new NotFoundException($"Fuel station with id = {request.FuelStationId} does not have fuel type with id = {request.FuelTypeId}");
        
        _fuelAtStationRepository.Remove(fuelAtStation);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}