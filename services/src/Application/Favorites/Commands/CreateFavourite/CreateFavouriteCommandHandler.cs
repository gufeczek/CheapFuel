using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Favorites.Commands.CreateFavourite;

public sealed class CreateFavouriteCommandHandler : IRequestHandler<CreateFavouriteCommand, SimpleUserFavouriteDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    private readonly IMapper _mapper;

    public CreateFavouriteCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _favoriteRepository = unitOfWork.Favorites;
        _fuelStationRepository = unitOfWork.FuelStations;
        _userPrincipalService = userPrincipalService;
        _mapper = mapper;
    }
    
    public async Task<SimpleUserFavouriteDto> Handle(CreateFavouriteCommand request, CancellationToken cancellationToken)
    {
        var username = _userPrincipalService.GetUserName()
                       ?? throw new UnauthorizedException("User is not logged in!");

        if (await _favoriteRepository.ExistsByUsernameAndFuelStationIdAsync(username, (long)request.FuelStationId!))
        {
            throw new ConflictException(
                $"User with username = {username} has already added fuel station with id = {request.FuelStationId} to his favourites");
        }
        
        var user = await _userRepository.GetByUsernameAsync(username) 
                   ?? throw new NotFoundException($"User not found for username = {username}");
        var fuelStation = await _fuelStationRepository.GetAsync((long)request.FuelStationId) 
                          ?? throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");

        var favourite = new Favorite
        {
            User = user,
            FuelStation = fuelStation
        };
        
        _favoriteRepository.Add(favourite);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<SimpleUserFavouriteDto>(favourite);
    }
}