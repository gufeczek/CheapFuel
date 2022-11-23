using Application.Models;
using MediatR;

namespace Application.Favorites.Queries.GetFavourite;

public sealed record GetFavouriteQuery(string? Username, long? FuelStationId) 
    : IRequest<SimpleUserFavouriteDto>;