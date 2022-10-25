using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed record FuelStationServiceDto(long Id, string Name);

public sealed class FuelStationServiceDtoProfile : Profile
{
    public FuelStationServiceDtoProfile()
    {
        CreateMap<FuelStationService, FuelStationServiceDto>();
    }
}