using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed record FuelTypeDto(long Id, string Name);

public sealed class FuelTypeDtoProfile : Profile
{
    public FuelTypeDtoProfile()
    {
        CreateMap<FuelType, FuelTypeDto>();
    }
}