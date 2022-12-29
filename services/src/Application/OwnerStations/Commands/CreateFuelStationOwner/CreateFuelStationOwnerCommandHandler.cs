using Application.Common.Exceptions;
using Application.Models.OwnedStations;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.OwnerStations.Commands.CreateFuelStationOwner;

public sealed class CreateFuelStationOwnerCommandHandler : IRequestHandler<CreateFuelStationOwnerCommand, OwnedStationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOwnedStationRepository _ownedStationRepository;
    private readonly IMapper _mapper;

    public CreateFuelStationOwnerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fuelStationRepository = unitOfWork.FuelStations;
        _userRepository = unitOfWork.Users;
        _ownedStationRepository = unitOfWork.OwnedStations;
        _mapper = mapper;
    }
    
    public async Task<OwnedStationDto> Handle(CreateFuelStationOwnerCommand request, CancellationToken cancellationToken)
    {
        var fuelStationId = request.FuelStationId!.Value;
        var userId = request.UserId!.Value;

        if (!await _fuelStationRepository.ExistsById(fuelStationId))
        {
            throw new NotFoundException($"Fuel station not found for id = {fuelStationId}");
        }

        if (await _ownedStationRepository.ExistsByUserIdAndFuelStationIdAsync(userId, fuelStationId))
        {
            throw new ConflictException("Fuel station is already owned by this user!");
        }
        
        var user = await _userRepository.GetAsync(userId)
                   ?? throw new NotFoundException($"User not found for userId = {userId}");
        
        if (user.Role == Role.User)
        {
            user.Role = Role.Owner;
        }

        var ownedFuelStation = new OwnedStation
        {
            User = user,
            FuelStationId = fuelStationId
        };

        _ownedStationRepository.Add(ownedFuelStation);
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<OwnedStationDto>(ownedFuelStation);
    }
}