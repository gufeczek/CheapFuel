using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class FuelStationDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public AddressDto? Address { get; set; }
    public FuelStationLocationDto Location { get; set; }
    public StationChainDto? StationChain { get; set; }
}

public sealed class FuelStationDtoProfile : Profile
{
    public FuelStationDtoProfile()
    {
        CreateMap<FuelStation, FuelStationDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<GeographicalCoordinates, FuelStationLocationDto>();
        CreateMap<StationChain, StationChainDto>();
    }
}