using Application.Models;
using MediatR;

namespace Application.FuelStations.Queries.GetFuelStationDetails;

public sealed record GetFuelStationDetailsQuery(long? Id)
    : IRequest<FuelStationDetailsDto>;