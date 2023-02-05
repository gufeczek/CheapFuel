using System.Linq.Expressions;
using Application.Models.Interfaces;
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

public sealed class FuelTypeDtoColumnSelector : IColumnSelector<FuelType>
{
    public Dictionary<string, Expression<Func<FuelType, object>>> ColumnSelector { get; } = new()
    {
        { nameof(FuelType.Id), r => r.Id },
        { nameof(FuelType.Name), r => r.Name }
    };
}