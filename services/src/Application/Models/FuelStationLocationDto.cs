using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class FuelStationLocationDto
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}

public sealed class FuelStationLocationDtoProfile : Profile
{
    public FuelStationLocationDtoProfile()
    {
        CreateMap<GeographicalCoordinates, FuelStationLocationDto>();
    }
}