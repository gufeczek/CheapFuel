using Application.Common;
using Application.Common.Exceptions;
using Application.Models;
using Application.Models.FuelStationDtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStations.Commands.CreateFuelStation;

public sealed class CreateFuelStationCommandHandler : IRequestHandler<CreateFuelStationCommand, FuelStationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStationChainRepository _stationChainRepository;
    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IFuelStationServiceRepository _fuelStationServiceRepository;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelAtStationRepository _fuelAtStationRepository;
    private readonly IServiceAtStationRepository _serviceAtStationRepository;
    private readonly IMapper _mapper;
    
    public CreateFuelStationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _stationChainRepository = unitOfWork.StationChains;
        _fuelTypeRepository = unitOfWork.FuelTypes;
        _fuelStationServiceRepository = unitOfWork.Services;
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelAtStationRepository = unitOfWork.FuelsAtStation;
        _serviceAtStationRepository = unitOfWork.ServicesAtStation;
        _mapper = mapper;
    }
    
    public async Task<FuelStationDto> Handle(CreateFuelStationCommand request, CancellationToken cancellationToken)
    {
        var dto = request.FuelStationDto!;
        await Validate(dto);

        var fuelStation = CreateFuelStationFromDto(dto);
        _fuelStationRepository.Add(fuelStation);
        await _unitOfWork.SaveAsync();
        
        IEnumerable<ServiceAtStation> serviceAtStations = CreateServiceAtStation(fuelStation.Id, dto.Services!);
        _serviceAtStationRepository.AddAll(serviceAtStations);
        
        IEnumerable<FuelAtStation> fuelAtStations = CreateFuelAtStation(fuelStation.Id, dto.FuelTypes!);
        _fuelAtStationRepository.AddAll(fuelAtStations);

        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<FuelStationDto>(fuelStation);
    }

    private async Task Validate(NewFuelStationDto dto)
    {
        if (!await _stationChainRepository.ExistsById(dto.StationChainId))
        {
            throw new NotFoundException($"Station chain not found for id = {dto.StationChainId}");
        }

        if (!await _fuelTypeRepository.ExistsAllById(dto.FuelTypes!))
        {
            throw new NotFoundException("One or more of given fuel types has not been found");
        }

        if (!await _fuelStationServiceRepository.ExistsAllById(dto.Services!))
        {
            throw new NotFoundException("One or more of given services has not been found");
        }

        if (CollectionUtils.HasDuplicates(dto.FuelTypes!))
        {
            throw new BadRequestException("Duplicate fuel types!");
        }
        
        if (CollectionUtils.HasDuplicates(dto.Services!))
        {
            throw new BadRequestException("Duplicate services!");
        }
    }

    private FuelStation CreateFuelStationFromDto(NewFuelStationDto dto)
    {
        return new FuelStation
        {
            Name = dto.Name,
            Address = new Address
            {
                City = dto.Address!.City,
                Street = dto.Address.Street,
                StreetNumber = dto.Address.StreetNumber,
                PostalCode = dto.Address.PostalCode!.Replace("-", "")
            },
            GeographicalCoordinates = new GeographicalCoordinates
            {
                Latitude = dto.GeographicalCoordinates!.Latitude!.Value,
                Longitude = dto.GeographicalCoordinates!.Longitude!.Value
            },
            StationChainId = dto.StationChainId
        };
    }

    private IEnumerable<ServiceAtStation> CreateServiceAtStation(long fuelStationId, IEnumerable<long> servicesIds)
    {
        return servicesIds.Select(id => new ServiceAtStation { FuelStationId = fuelStationId, ServiceId = id });
    }

    private IEnumerable<FuelAtStation> CreateFuelAtStation(long fuelStationId, IEnumerable<long> fuelTypeIds)
    {
        return fuelTypeIds.Select(id => new FuelAtStation { FuelStationId = fuelStationId, FuelTypeId = id });
    }
}