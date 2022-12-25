using Application.Common;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models.FuelPriceDtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelPrices.Commands.UpdateFuelPriceByUser;

public class UpdateFuelPriceByUserCommandHandler : IRequestHandler<UpdateFuelPriceByUserCommand, IEnumerable<FuelPriceDto>>
{
    private const double AllowedDistanceThreshold = 200.0;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IOwnedStationRepository _ownedStationRepository;
    private readonly IFuelPriceRepository _fuelPriceRepository;
    private readonly IFuelAtStationRepository _fuelAtStationRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    private readonly IMapper _mapper;

    public UpdateFuelPriceByUserCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelPriceRepository = unitOfWork.FuelPrices;
        _ownedStationRepository = unitOfWork.OwnedStations;
        _fuelAtStationRepository = unitOfWork.FuelsAtStation;
        _userPrincipalService = userPrincipalService;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<FuelPriceDto>> Handle(UpdateFuelPriceByUserCommand request, CancellationToken cancellationToken)
    {
        var username = _userPrincipalService.GetUserName()
                       ?? throw new UnauthorizedException("User is not logged in!");
        var user = await _userRepository.GetByUsernameAsync(username) 
                   ?? throw new NotFoundException($"User not found for username = {username}");

        var dto = request.FuelPricesAtStation!;
        var fuelStation = await _fuelStationRepository.GetFuelStationWithFuelTypesAsync((long)dto.FuelStationId!)
                          ?? throw new NotFoundException($"Fuel station not found for id = {dto.FuelStationId}");

        CheckForDistance(fuelStation, dto);
        CheckForDuplicateFuelTypes(dto);
        await CheckIfAllFuelTypesExistsAtFuelStation(dto, fuelStation.Id);

        if (user.Role != Role.Admin && !await _ownedStationRepository.ExistsByUserIdAndFuelStationIdAsync(user.Id, (long)dto.FuelStationId!))
        {
            throw new NotFoundException($"Not found fuel station with id = {dto.FuelStationId} and owner with username = {username}");
        }
        
        IEnumerable<FuelPrice> fuelPrices = MapToFuelPrice(dto, user.Id);
        
        _fuelPriceRepository.AddAll(fuelPrices);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<IEnumerable<FuelPriceDto>>(fuelPrices);
    }

    private void CheckForDistance(FuelStation fuelStation, NewFuelPricesAtStationWithLocation dto)
    {
        var distance = Utils.GetDistance(
            (double)fuelStation.GeographicalCoordinates!.Longitude,
            (double)fuelStation.GeographicalCoordinates!.Latitude, 
            dto.UserLongitude!.Value, 
            dto.UserLatitude!.Value);

        if (distance > AllowedDistanceThreshold)
        {
            throw new BadRequestException("User location is too far from fuel station location");
        }
    }

    private void CheckForDuplicateFuelTypes(NewFuelPricesAtStationWithLocation dto)
    {
        var anyDuplicate = dto.FuelPrices!
            .GroupBy(f => f.FuelTypeId)
            .Any(g => g.Count() > 1);

        if (anyDuplicate)
        {
            throw new BadRequestException("Request should not contains duplicate fuel types!");
        }
    }

    private async Task CheckIfAllFuelTypesExistsAtFuelStation(NewFuelPricesAtStationWithLocation dto, long fuelStationId)
    {
        var fuelTypeIds = dto.FuelPrices!.Select(f => (long)f.FuelTypeId!).ToList();
        var count = await _fuelAtStationRepository.CountAllByFuelStationIdAndFuelTypesIdsAsync(fuelStationId, fuelTypeIds);

        if (fuelTypeIds.Count > count)
        {
            throw new NotFoundException("Some of given fuel types are not in the fuel station");
        }
    }

    private IEnumerable<FuelPrice> MapToFuelPrice(NewFuelPricesAtStationWithLocation dto, long userId)
    {
        var fuelPrices = new List<FuelPrice>();

        foreach (var priceDto in dto.FuelPrices!)
        {
            var fuelPrice = new FuelPrice
            {
                Price = (decimal)priceDto.Price!,
                Available = priceDto.Available,
                Status = FuelPriceStatus.Accepted,
                Priority = true,
                FuelStationId = dto.FuelStationId,
                FuelTypeId = priceDto.FuelTypeId,
                UserId = userId
            };
            fuelPrices.Add(fuelPrice);
        }
        
        return fuelPrices;
    }
}