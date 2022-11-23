using MediatR;

namespace Application.Favorites.Commands.DeleteFavourite;

public sealed record DeleteFavouriteCommand(long? FuelStationId) : IRequest<Unit>;