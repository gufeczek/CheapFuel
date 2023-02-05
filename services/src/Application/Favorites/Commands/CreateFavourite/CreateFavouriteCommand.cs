using Application.Models;
using MediatR;

namespace Application.Favorites.Commands.CreateFavourite;

public sealed record CreateFavouriteCommand(long? FuelStationId) 
    : IRequest<SimpleUserFavouriteDto>;