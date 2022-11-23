using MediatR;

namespace Application.Favorites.Commands.DeleteFavorite;

public sealed record DeleteFavoriteCommand(long? FuelStationId) : IRequest<Unit>;