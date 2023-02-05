using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Favorites.Queries.GetFavourite;

public sealed class GetFavouriteQueryHandler : IRequestHandler<GetFavouriteQuery, SimpleUserFavouriteDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IMapper _mapper;

    public GetFavouriteQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = unitOfWork.Users;
        _fuelStationRepository = unitOfWork.FuelStations;
        _favoriteRepository = unitOfWork.Favorites;
        _mapper = mapper;
    }
    
    public async Task<SimpleUserFavouriteDto> Handle(GetFavouriteQuery request, CancellationToken cancellationToken)
    {
        if (!await _userRepository.ExistsByUsername(request.Username!))
        {
            throw new NotFoundException($"User not found for username = {request.Username}");
        }
        
        if (!await _fuelStationRepository.ExistsById((long)request.FuelStationId!))
        {
            throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");
        }
        
        var userFavourite = await _favoriteRepository.GetByUsernameAndFuelStationIdAsync(request.Username!, (long)request.FuelStationId!)
                            ?? throw new NotFoundException($"User with username {request.Username} does not have fuel station with id {request.FuelStationId} in his favourites");

        return _mapper.Map<SimpleUserFavouriteDto>(userFavourite);
    }
}