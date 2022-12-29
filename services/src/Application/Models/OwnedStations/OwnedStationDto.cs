using AutoMapper;
using Domain.Entities;

namespace Application.Models.OwnedStations;

public sealed class OwnedStationDto
{
    public long FuelStationId { get; set; }
    public long UserId { get; set; }
}

public sealed class OwnedStationDtoProfile : Profile
{
    public OwnedStationDtoProfile()
    {
        CreateMap<OwnedStation, OwnedStationDto>();
    }
}