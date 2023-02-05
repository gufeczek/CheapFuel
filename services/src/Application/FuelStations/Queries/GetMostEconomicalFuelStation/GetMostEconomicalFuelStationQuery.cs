using Application.Models.FuelStationDtos;
using MediatR;

namespace Application.FuelStations.Queries.GetMostEconomicalFuelStation;

public sealed record GetMostEconomicalFuelStationQuery(
        double? UserLongitude,
        double? UserLatitude,
        double? FuelConsumption,
        double? FuelAmountToBuy,
        long? FuelTypeId) 
    : IRequest<MostEconomicalFuelStationDto>;