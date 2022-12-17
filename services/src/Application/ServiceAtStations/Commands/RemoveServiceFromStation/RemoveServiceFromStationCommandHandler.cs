using Application.Common.Authentication;
using Application.Common.Exceptions;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.ServiceAtStations.Commands.RemoveServiceFromStation;

public sealed class RemoveServiceFromStationCommandHandler : IRequestHandler<RemoveServiceFromStationCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelStationServiceRepository _fuelStationServiceRepository;
    private readonly IServiceAtStationRepository _serviceAtStationRepository;
    private readonly IOwnedStationRepository _ownedStationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    
    public RemoveServiceFromStationCommandHandler(IUnitOfWork unitOfWork,  IUserPrincipalService userPrincipalService)
    {
        _unitOfWork = unitOfWork;
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelStationServiceRepository = unitOfWork.Services;
        _serviceAtStationRepository = unitOfWork.ServicesAtStation;
        _ownedStationRepository = unitOfWork.OwnedStations;
        _userRepository = unitOfWork.Users;
        _userPrincipalService = userPrincipalService;
    }


    public async Task<Unit> Handle(RemoveServiceFromStationCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequest(request);
        
        var fuelAtStation = await _serviceAtStationRepository.GetAsync(request.FuelStationId, request.ServiceId)
                            ?? throw new NotFoundException($"Fuel station with id = {request.FuelStationId} does not have service with id = {request.ServiceId}");
        
        _serviceAtStationRepository.Remove(fuelAtStation);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }

    private async Task ValidateRequest(RemoveServiceFromStationCommand request)
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

        if (!await _fuelStationServiceRepository.ExistsById(request.ServiceId))
        {
            throw new NotFoundException($"Service not found for id = {request.ServiceId}");
        }
    }
}