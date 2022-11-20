using System.Linq.Expressions;
using Application.Models.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class FuelStationServiceDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
}

public sealed class FuelStationServiceDtoProfile : Profile
{
    public FuelStationServiceDtoProfile()
    {
        CreateMap<FuelStationService, FuelStationServiceDto>();
    }
}

public sealed class FuelStationServiceDtoColumnSelector : IColumnSelector<FuelStationService>
{
    public Dictionary<string, Expression<Func<FuelStationService, object>>> ColumnSelector { get; } = new()
    {
        { nameof(FuelType.Id), r => r.Id },
        { nameof(FuelType.Name), r => r.Name }
    };
}