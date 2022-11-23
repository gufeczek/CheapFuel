using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class SimpleUserFavouriteDto
{
    public long FuelStationId { get; set; }
    public string? Username { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class SimpleUserFavouriteDtoProfile : Profile 
{
    public SimpleUserFavouriteDtoProfile()
    {
        CreateMap<Favorite, SimpleUserFavouriteDto>()
            .ForMember(
                d => d.Username, 
                o => o.MapFrom(s => s.User!.Username));
    }
}