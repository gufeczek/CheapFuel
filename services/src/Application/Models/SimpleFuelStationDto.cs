using System.Linq.Expressions;
using Application.Models.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed record SimpleFuelStationDto(
    long Id,
    string StationChainName,
    decimal Price,
    DateTime LastFuelPriceUpdate,
    decimal Latitude,
    decimal Longitude);

public sealed class SimpleFuelStationDtoProfile : Profile
{
    public SimpleFuelStationDtoProfile()
    {
        CreateMap<FuelStation, SimpleFuelStationDto>()
            .ForCtorParam("StationChainName", o => o.MapFrom(s => s.StationChain!.Name))
            .ForCtorParam("Price", o => o.MapFrom(s => s.FuelPrices.First().Price))
            .ForCtorParam("LastFuelPriceUpdate", o => o.MapFrom(s => s.FuelPrices.First().CreatedAt))
            .ForCtorParam("Latitude", o => o.MapFrom(s => s.GeographicalCoordinates!.Latitude))
            .ForCtorParam("Longitude", o => o.MapFrom(s => s.GeographicalCoordinates!.Longitude));
    }
}

public sealed class SimpleFuelStationDtoColumnSelector : IColumnSelector<FuelStation>
{
    public Dictionary<string, Expression<Func<FuelStation, object>>> ColumnSelector { get; } = new()
    {
        { nameof(FuelStation.Id), r => r.Id },
        { nameof(SimpleFuelStationDto.StationChainName), r => r.StationChain!.Name! },
        { nameof(SimpleFuelPriceDto.Price), r => r.FuelPrices.First().Price }
    };
}