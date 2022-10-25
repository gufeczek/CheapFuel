using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed record FuelTypeDto(long Id, string Name);

public sealed class FuelTypeProfile : Profile
{
    public FuelTypeProfile()
    {
        CreateMap<FuelType, FuelTypeDto>();
    }
}