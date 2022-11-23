using Application.Models;
using MediatR;

namespace Application.Favorites.Commands.CreateFavorite;

public sealed record CreateFavouriteCommand(long? FuelStationId) 
    : IRequest<SimpleUserFavouriteDto>;