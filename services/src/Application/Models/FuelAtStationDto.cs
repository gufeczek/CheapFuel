using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class FuelAtStationDto
{
    public long FuelStationId { get; set; }
    public FuelTypeDto? FuelType { get; set; }
}

public sealed class FuelAtStationDtoProfile : Profile
{
    public FuelAtStationDtoProfile()
    {
        CreateMap<FuelAtStation, FuelAtStationDto>();
        CreateMap<FuelType, FuelTypeDto>();
    }
}