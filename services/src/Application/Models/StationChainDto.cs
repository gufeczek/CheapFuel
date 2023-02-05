using System.Linq.Expressions;
using Application.Models.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed record StationChainDto(long Id, string Name);

public sealed class StationChainDtoProfile : Profile
{
    public StationChainDtoProfile()
    {
        CreateMap<StationChain, StationChainDto>();
    }
}

public sealed class StationChainDtoColumnSelector : IColumnSelector<StationChain>
{
    public Dictionary<string, Expression<Func<StationChain, object>>> ColumnSelector { get; } = new()
    {
        { nameof(StationChain.Id), r => r.Id },
        { nameof(StationChain.Name), r => r.Name }
    };
}
    