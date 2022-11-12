using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed record SimpleMapFuelStationDto(
    long Id,
    string StationChainName,
    decimal Price,
    decimal Latitude,
    decimal Longitude);

public sealed class SimpleMapFuelStationDtoProfile : Profile
{
    public SimpleMapFuelStationDtoProfile()
    {
        CreateMap<FuelStation, SimpleMapFuelStationDto>()
            .ForCtorParam("StationChainName", o => o.MapFrom(s => s.StationChain!.Name))
            .ForCtorParam("Price", o => o.MapFrom(s => s.FuelPrices.First().Price))
            .ForCtorParam("Latitude", o => o.MapFrom(s => s.GeographicalCoordinates!.Latitude))
            .ForCtorParam("Longitude", o => o.MapFrom(s => s.GeographicalCoordinates!.Longitude));
    }
}