using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class ServiceAtStationDto
{
    public long FuelStationId { get; set; }
    public FuelStationServiceDto? Service { get; set; }
}

public sealed class ServiceAtStationDtoProfile : Profile
{
    public ServiceAtStationDtoProfile()
    {
        CreateMap<ServiceAtStation, ServiceAtStationDto>();
        CreateMap<FuelStationService, FuelStationServiceDto>();
    }
}