using Application.Common.Authentication;
using Application.Common.Exceptions;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelAtStations.Commands.RemoveFuelFromStation;

public sealed class RemoveFuelFromStationCommandHandler : IRequestHandler<RemoveFuelFromStationCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IFuelAtStationRepository _fuelAtStationRepository;
    private readonly IOwnedStationRepository _ownedStationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    
    public RemoveFuelFromStationCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService)
    {
        _unitOfWork = unitOfWork;
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelTypeRepository = unitOfWork.FuelTypes;
        _fuelAtStationRepository = unitOfWork.FuelsAtStation;
        _ownedStationRepository = unitOfWork.OwnedStations;
        _userRepository = unitOfWork.Users;
        _userPrincipalService = userPrincipalService;
    }
    
    public async Task<Unit> Handle(RemoveFuelFromStationCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequest(request);
        
        var fuelAtStation = await _fuelAtStationRepository.GetAsync(request.FuelStationId, request.FuelTypeId)
                            ?? throw new NotFoundException($"Fuel station with id = {request.FuelStationId} does not have fuel type with id = {request.FuelTypeId}");
        
        _fuelAtStationRepository.Remove(fuelAtStation);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }

    private async Task ValidateRequest(RemoveFuelFromStationCommand request)
    {
        var username = _userPrincipalService.GetUserName()
                       ?? throw new UnauthorizedException("User is not logged in!");
        var user = await _userRepository.GetByUsernameAsync(username)
                   ?? throw new NotFoundException($"User not found for username = {username}");
        
        if (user.Role != Role.Admin && !await _ownedStationRepository.ExistsByUserIdAndFuelStationIdAsync(user.Id, request.FuelStationId))
        {
            throw new ForbiddenException("User is not an owner of this station!");
        }
        
        if (!await _fuelStationRepository.ExistsById(request.FuelStationId))
        {
            throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");
        }

        if (!await _fuelTypeRepository.ExistsById(request.FuelTypeId))
        {
            throw new NotFoundException($"Fuel type not found for id = {request.FuelTypeId}");
        }
    }
}