using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelAtStations.Commands.AddFuelToStation;

public sealed class AddFuelToStationCommandHandler : IRequestHandler<AddFuelToStationCommand, FuelAtStationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IFuelAtStationRepository _fuelAtStationRepository;
    private readonly IOwnedStationRepository _ownedStationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    private readonly IMapper _mapper;

    public AddFuelToStationCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelTypeRepository = unitOfWork.FuelTypes;
        _fuelAtStationRepository = unitOfWork.FuelsAtStation;
        _ownedStationRepository = unitOfWork.OwnedStations;
        _userRepository = unitOfWork.Users;
        _userPrincipalService = userPrincipalService;
        _mapper = mapper;
    }
    
    public async Task<FuelAtStationDto> Handle(AddFuelToStationCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequest(request);
        
        var fuelStation = await _fuelStationRepository.GetAsync(request.FuelStationId)
                          ?? throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");
        var fuelType = await _fuelTypeRepository.GetAsync(request.FuelTypeId)
                       ?? throw new NotFoundException($"Fuel type not found for id = {request.FuelTypeId}");

        var fuelAtStation = new FuelAtStation
        {
            FuelStation = fuelStation,
            FuelType = fuelType
        };
        
        _fuelAtStationRepository.Add(fuelAtStation);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<FuelAtStationDto>(fuelAtStation);
    }

    private async Task ValidateRequest(AddFuelToStationCommand request)
    {
        var username = _userPrincipalService.GetUserName()
                       ?? throw new UnauthorizedException("User is not logged in!");
        var user = await _userRepository.GetByUsernameAsync(username)
                   ?? throw new NotFoundException($"User not found for username = {username}");
        
        if (user.Role != Role.Admin && !await _ownedStationRepository.ExistsByUserIdAndFuelStationIdAsync(user.Id, request.FuelStationId))
        {
            throw new ForbiddenException("User is not an owner of this station!");
        }
        
        if (await _fuelAtStationRepository.ExistsAsync(request.FuelStationId, request.FuelTypeId))
        {
            throw new ConflictException($"Fuel station with id = {request.FuelStationId} already has fuel type with id = {request.FuelTypeId}");
        }
    }
}