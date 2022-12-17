using Application.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.ServiceAtStations.Commands.RemoveServiceFromStation;

public sealed class RemoveServiceFromStationCommandHandler : IRequestHandler<RemoveServiceFromStationCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelStationServiceRepository _fuelStationServiceRepository;
    private readonly IServiceAtStationRepository _serviceAtStationRepository;

    public RemoveServiceFromStationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelStationServiceRepository = unitOfWork.Services;
        _serviceAtStationRepository = unitOfWork.ServicesAtStation;
    }


    public async Task<Unit> Handle(RemoveServiceFromStationCommand request, CancellationToken cancellationToken)
    {
        if (!await _fuelStationRepository.ExistsById(request.FuelStationId))
        {
            throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");
        }

        if (!await _fuelStationServiceRepository.ExistsById(request.ServiceId))
        {
            throw new NotFoundException($"Service not found for id = {request.ServiceId}");
        }

        var fuelAtStation = await _serviceAtStationRepository.GetAsync(request.FuelStationId, request.ServiceId)
                            ?? throw new NotFoundException($"Fuel station with id = {request.FuelStationId} does not have service with id = {request.ServiceId}");
        
        _serviceAtStationRepository.Remove(fuelAtStation);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}