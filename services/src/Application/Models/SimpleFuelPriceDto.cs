using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class SimpleFuelPriceDto
{
    public decimal Price { get; set; }
    public bool Available { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class SimpleFuelPriceDtoProfile : Profile
{
    public SimpleFuelPriceDtoProfile()
    {
        CreateMap<FuelPrice, FuelTypeDto>();
    }
}