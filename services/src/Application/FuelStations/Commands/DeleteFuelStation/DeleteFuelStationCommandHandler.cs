using Application.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStations.Commands.DeleteFuelStation;

public sealed class DeleteFuelStationCommandHandler : IRequestHandler<DeleteFuelStationCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelAtStationRepository _fuelAtStationRepository;
    private readonly IServiceAtStationRepository _serviceAtStationRepository;
    private readonly IOpeningClosingTimeRepository _openingClosingTimeRepository;
    private readonly IFuelPriceRepository _fuelPriceRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IOwnedStationRepository _ownedStationRepository;
    
    public DeleteFuelStationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelAtStationRepository = unitOfWork.FuelsAtStation;
        _serviceAtStationRepository = unitOfWork.ServicesAtStation;
        _openingClosingTimeRepository = unitOfWork.OpeningClosingTimes;
        _fuelPriceRepository = unitOfWork.FuelPrices;
        _reviewRepository = unitOfWork.Reviews;
        _ownedStationRepository = unitOfWork.OwnedStations;
    }
    
    public async Task<Unit> Handle(DeleteFuelStationCommand request, CancellationToken cancellationToken)
    {
        var fuelStationId = request.FuelStationId!.Value;
        var fuelStation = await _fuelStationRepository.GetAsync(fuelStationId) 
                          ?? throw new NotFoundException($"Fuel station not found for id = {fuelStationId}");

        await _fuelAtStationRepository.RemoveAllByFuelStationId(fuelStationId);
        await _serviceAtStationRepository.RemoveAllByFuelStationId(fuelStationId);
        await _openingClosingTimeRepository.RemoveAllByFuelStationId(fuelStationId);
        await _fuelPriceRepository.RemoveAllByFuelStationId(fuelStationId);
        await _reviewRepository.RemoveAllByFuelStationId(fuelStationId);
        await _ownedStationRepository.RemoveAllByFuelStationId(fuelStationId);
        _fuelStationRepository.Remove(fuelStation);

        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}