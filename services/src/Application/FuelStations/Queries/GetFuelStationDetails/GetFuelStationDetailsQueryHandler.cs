using Application.Common;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStations.Queries.GetFuelStationDetails;

public sealed class GetFuelStationDetailsQueryHandler : IRequestHandler<GetFuelStationDetailsQuery, FuelStationDetailsDto>
{
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelPriceRepository _fuelPriceRepository;
    private readonly IMapper _mapper;
    
    public GetFuelStationDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelPriceRepository = unitOfWork.FuelPrices;
        _mapper = mapper;
    }

    public async Task<FuelStationDetailsDto> Handle(GetFuelStationDetailsQuery request, CancellationToken cancellationToken)
    {
        var fuelStation = await _fuelStationRepository.GetFuelStationWithAllDetails(request.Id.OrElseThrow())
                          ?? throw new NotFoundException($"Fuel station not found for id = {request.Id}");

        var fuelPrices = FetchMostRecentFuelPrices(fuelStation);
        return Map(fuelStation, fuelPrices);
    }

    private IEnumerable<FuelPrice?> FetchMostRecentFuelPrices(FuelStation fuelStation)
    {
        return fuelStation.FuelTypes
            .Select(async f => await _fuelPriceRepository.GetMostRecentPrice(fuelStation.Id, f.FuelTypeId))
            .Select(t => t.Result)
            .Where(f => f is not null)
            .ToList();
    }

    private FuelStationDetailsDto Map(FuelStation fuelStation, IEnumerable<FuelPrice?> fuelPrices)
    {
        var services = fuelStation.ServiceAtStations.Select(s => s.Service);
        
        var fuelTypesWithPrice = new List<FuelTypeWithPriceDto>();
        var fuelPricesList = fuelPrices.ToList();
        
        foreach (var fuelType in fuelStation.FuelTypes)
        {
            var price = fuelPricesList.FirstOrDefault(f => f!.FuelTypeId == fuelType.FuelTypeId);
            var priceDto = price is null ? null : _mapper.Map<SimpleFuelPriceDto>(price);
            
            var fuelTypeWithPrice = new FuelTypeWithPriceDto
            {
                Id = fuelType.FuelTypeId,
                Name = fuelType.FuelType!.Name!,
                FuelPrice = priceDto
            };
            
            fuelTypesWithPrice.Add(fuelTypeWithPrice);
        }
        
        return new FuelStationDetailsDto
        {
            Id = fuelStation.Id,
            Name = fuelStation.Name,
            Address = _mapper.Map<AddressDto>(fuelStation.Address),
            Location = _mapper.Map<FuelStationLocationDto>(fuelStation.GeographicalCoordinates),
            StationChain = _mapper.Map<StationChainDto>(fuelStation.StationChain),
            OpeningClosingTimes = _mapper.Map<IEnumerable<OpeningClosingTimeDto>>(fuelStation.OpeningClosingTimes),
            Services = _mapper.Map<IEnumerable<FuelStationServiceDto>>(services),
            FuelTypes = fuelTypesWithPrice
        };
    }
}