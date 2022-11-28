using Application.Models.FuelPriceDtos;
using MediatR;

namespace Application.FuelPrices.Commands.UpdateFuelPriceByOwner;

public sealed record UpdateFuelPriceByOwnerCommand(NewFuelPricesAtStationDto? FuelPricesAtStationDto) 
    : IRequest<IEnumerable<FuelPriceDto>>;
