using System.Linq.Expressions;
using Application.Models.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class UserFavouriteDto
{
    public long FuelStationId { get; set; }
    public string? Username { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? StationChain { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class UserFavouriteDtoProfile : Profile
{
    public UserFavouriteDtoProfile()
    {
        CreateMap<Favorite, UserFavouriteDto>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.User!.Username))
            .ForMember(d => d.City, o => o.MapFrom(s => s.FuelStation!.Address!.City))
            .ForMember(d => d.Street, o => o.MapFrom(s => s.FuelStation!.Address!.Street))
            .ForMember(d => d.StationChain, o => o.MapFrom(s => s.FuelStation!.StationChain!.Name))
            .ForMember(d => d.Latitude, o => o.MapFrom(s => s.FuelStation!.GeographicalCoordinates!.Latitude))
            .ForMember(d => d.Longitude, o => o.MapFrom(s => s.FuelStation!.GeographicalCoordinates!.Longitude));
    }
}

public sealed class UserFavouriteDtoColumnSelector : IColumnSelector<Favorite>
{
    public Dictionary<string, Expression<Func<Favorite, object>>> ColumnSelector { get; } = new()
    {
        { nameof(Favorite.FuelStationId), r => r.FuelStationId }
    };
}