using Application.Models.FuelPriceDtos;
using MediatR;

namespace Application.FuelPrices.Commands.UpdateFuelPriceByUser;

public sealed record UpdateFuelPriceByUserCommand(NewFuelPricesAtStationWithLocation? FuelPricesAtStation) 
    : IRequest<IEnumerable<FuelPriceDto>>;