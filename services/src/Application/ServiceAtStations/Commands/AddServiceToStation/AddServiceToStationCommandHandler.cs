using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.ServiceAtStations.Commands.AddServiceToStation;

public sealed class AddServiceToStationCommandHandler : IRequestHandler<AddServiceToStationCommand, ServiceAtStationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelStationServiceRepository _fuelStationServiceRepository;
    private readonly IServiceAtStationRepository _serviceAtStationRepository;
    private readonly IOwnedStationRepository _ownedStationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    private readonly IMapper _mapper;

    public AddServiceToStationCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelStationServiceRepository = unitOfWork.Services;
        _serviceAtStationRepository = unitOfWork.ServicesAtStation;
        _ownedStationRepository = unitOfWork.OwnedStations;
        _userRepository = unitOfWork.Users;
        _userPrincipalService = userPrincipalService;
        _mapper = mapper;
    }
    
    public async Task<ServiceAtStationDto> Handle(AddServiceToStationCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequest(request);
        
        var fuelStation = await _fuelStationRepository.GetAsync(request.FuelStationId)
                          ?? throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");
        var service = await _fuelStationServiceRepository.GetAsync(request.ServiceId)
                       ?? throw new NotFoundException($"Service not found for id = {request.ServiceId}");

        var serviceAtStation = new ServiceAtStation
        {
            FuelStation = fuelStation,
            Service = service
        };
        
        _serviceAtStationRepository.Add(serviceAtStation);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ServiceAtStationDto>(serviceAtStation);
    }

    private async Task ValidateRequest(AddServiceToStationCommand request)
    {
        var username = _userPrincipalService.GetUserName()
                       ?? throw new UnauthorizedException("User is not logged in!");
        var user = await _userRepository.GetByUsernameAsync(username)
                   ?? throw new NotFoundException($"User not found for username = {username}");
        
        if (user.Role != Role.Admin && !await _ownedStationRepository.ExistsByUserIdAndFuelStationIdAsync(user.Id, request.FuelStationId))
        {
            throw new ForbiddenException("User is not an owner of this station!");
        }
        
        if (await _serviceAtStationRepository.ExistsAsync(request.FuelStationId, request.ServiceId))
        {
            throw new ConflictException($"Fuel station with id = {request.FuelStationId} already has service with id = {request.ServiceId}");
        }
    }
}