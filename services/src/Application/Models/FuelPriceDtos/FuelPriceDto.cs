using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models.FuelPriceDtos;

public sealed class FuelPriceDto
{
    public decimal Price { get; set; }
    public bool? Available { get; set; }
    public FuelPriceStatus Status { get; set; }
    public long? FuelStationId { get; set; }
    public long? FuelTypeId { get; set; }
}

public sealed class FuelPriceDtoProfile : Profile
{
    public FuelPriceDtoProfile()
    {
        CreateMap<FuelPrice, FuelPriceDto>();
    }
}