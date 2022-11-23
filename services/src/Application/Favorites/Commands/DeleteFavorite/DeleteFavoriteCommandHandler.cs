using Application.Common.Authentication;
using Application.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Favorites.Commands.DeleteFavorite;

public sealed class DeleteFavoriteCommandHandler : IRequestHandler<DeleteFavoriteCommand, Unit>
{
    private IUnitOfWork _unitOfWork;
    private IUserRepository _userRepository;
    private IFuelStationRepository _fuelStationRepository;
    private IFavoriteRepository _favoriteRepository;
    private readonly IUserPrincipalService _userPrincipalService;

    public DeleteFavoriteCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _fuelStationRepository = unitOfWork.FuelStations;
        _favoriteRepository = unitOfWork.Favorites;
        _userPrincipalService = userPrincipalService;
    }

    public async Task<Unit> Handle(DeleteFavoriteCommand request, CancellationToken cancellationToken)
    {
        var username = _userPrincipalService.GetUserName()
                       ?? throw new UnauthorizedException("User is not logged in!");

        if (!await _userRepository.ExistsByUsername(username))
        {
            throw new NotFoundException($"User not found for username = {username}");
        }

        if (!await _fuelStationRepository.ExistsById(request.FuelStationId))
        {
            throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");
        }

        var userFavourite = await _favoriteRepository.GetByUsernameAndFuelStationIdAsync(username, request.FuelStationId)
                            ?? throw new NotFoundException($"User with username {username} does not have fuel station with id {request.FuelStationId} in his favourites");
        
        _favoriteRepository.Remove(userFavourite);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}