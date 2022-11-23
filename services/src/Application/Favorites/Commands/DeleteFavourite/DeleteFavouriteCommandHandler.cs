using Application.Common.Authentication;
using Application.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Favorites.Commands.DeleteFavourite;

public sealed class DeleteFavouriteCommandHandler : IRequestHandler<DeleteFavouriteCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IUserPrincipalService _userPrincipalService;

    public DeleteFavouriteCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _fuelStationRepository = unitOfWork.FuelStations;
        _favoriteRepository = unitOfWork.Favorites;
        _userPrincipalService = userPrincipalService;
    }

    public async Task<Unit> Handle(DeleteFavouriteCommand request, CancellationToken cancellationToken)
    {
        var username = _userPrincipalService.GetUserName()
                       ?? throw new UnauthorizedException("User is not logged in!");

        if (!await _userRepository.ExistsByUsername(username))
        {
            throw new NotFoundException($"User not found for username = {username}");
        }

        if (!await _fuelStationRepository.ExistsById((long)request.FuelStationId!))
        {
            throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");
        }

        var userFavourite = await _favoriteRepository.GetByUsernameAndFuelStationIdAsync(username, (long)request.FuelStationId!)
                            ?? throw new NotFoundException($"User with username {username} does not have fuel station with id {request.FuelStationId} in his favourites");
        
        _favoriteRepository.Remove(userFavourite);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}