using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class FuelTypeWithPriceDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public SimpleFuelPriceDto? FuelPrice { get; set; }
}

public sealed class FuelTypeWithPriceDtoProfile : Profile
{
    public FuelTypeWithPriceDtoProfile()
    {
        CreateMap<FuelType, FuelTypeWithPriceDto>();
        CreateMap<FuelPrice, SimpleFuelPriceDto>();
    }
}