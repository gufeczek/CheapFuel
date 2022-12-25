using Application.Common;
using Application.Common.Exceptions;
using Application.Models.FuelStationDtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStations.Queries.GetMostEconomicalFuelStation;

public class GetMostEconomicalFuelStationQueryHandler : IRequestHandler<GetMostEconomicalFuelStationQuery, MostEconomicalFuelStationDto>
{
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelTypeRepository _fuelTypeRepository;

    public GetMostEconomicalFuelStationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelTypeRepository = unitOfWork.FuelTypes;
    }
    
    public async Task<MostEconomicalFuelStationDto> Handle(GetMostEconomicalFuelStationQuery request, CancellationToken cancellationToken)
    {
        var fuelTypeId = request.FuelTypeId!.Value;

        if (!await _fuelTypeRepository.ExistsById(fuelTypeId))
        {
            throw new NotFoundException($"Fuel type not found for id = {fuelTypeId}");
        }
        
        var fuelStations = await _fuelStationRepository.GetFuelStationWithPricesAsync(fuelTypeId);
        var mostEconomicalFuelStation = fuelStations
            .Where(fs => fs.FuelPrices.Any(fp => fp.FuelTypeId == fuelTypeId))
            .MinBy(fs => CalculateCost(request, fs, fuelTypeId));

        if (mostEconomicalFuelStation is null)
        {
            throw new NotFoundException("No fuel station could be found");
        }
        
        return new MostEconomicalFuelStationDto
        {
            FuelStationId = mostEconomicalFuelStation.Id,
            Cost = CalculateCost(request, mostEconomicalFuelStation, fuelTypeId)
        };
    }

    private double CalculateCost(GetMostEconomicalFuelStationQuery request, FuelStation fuelStation, long fuelTypeId)
    {
        var fuelPriceAtStation = fuelStation.FuelPrices.FirstOrDefault(fp => fp.FuelTypeId == fuelTypeId)?.Price;
        var distance = Utils.GetDistance(
            (double)fuelStation.GeographicalCoordinates!.Longitude,
            (double)fuelStation.GeographicalCoordinates!.Latitude,
            request.UserLongitude!.Value,
            request.UserLatitude!.Value) / 1000.0;
        Console.WriteLine(distance);
        var costToDestination = (request.FuelConsumption / 100.0) * distance * (double)fuelPriceAtStation!;
        var costOfFuelToBuy = request.FuelAmountToBuy * (double)fuelPriceAtStation;
        return (double)costToDestination! + (double)costOfFuelToBuy!;
    }
}