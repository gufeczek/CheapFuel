using Application.Common.Exceptions;
using Application.Models;
using Application.Models.Filters;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStations.Queries.GetAllFuelStationForMap;

public sealed class GetAllFuelStationsForMapQueryHandler 
    : IRequestHandler<GetAllFuelStationsForMapQuery, IEnumerable<SimpleFuelStationDto>>
{
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IFuelStationServiceRepository _fuelStationServiceRepository;
    private readonly IStationChainRepository _stationChainRepository;
    private readonly IMapper _mapper;
    
    public GetAllFuelStationsForMapQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelTypeRepository = unitOfWork.FuelTypes;
        _fuelStationServiceRepository = unitOfWork.Services;
        _stationChainRepository = unitOfWork.StationChains;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<SimpleFuelStationDto>> Handle(GetAllFuelStationsForMapQuery request, CancellationToken cancellationToken)
    {
        var filter = request.FilterDto;

        await ValidateFilters(filter!);

        var fuelStations = await _fuelStationRepository.GetFuelStationsWithFuelPrice(
            filter!.FuelTypeId,
            filter.ServicesIds,
            filter.StationChainsIds,
            filter.MinPrice,
            filter.MaxPrice);

        fuelStations = ApplyPriceFilter(fuelStations, filter);
        return _mapper.Map<IEnumerable<SimpleFuelStationDto>>(fuelStations);
    }

    private async Task ValidateFilters(FuelStationFilterDto filterDto)
    {
        var validationErrors = new List<string>();
        
        if (!await _fuelTypeRepository.ExistsById(filterDto.FuelTypeId))
        {
            validationErrors.Add($"Fuel type not found for id = {filterDto.FuelTypeId}");
        }

        if (filterDto.StationChainsIds is not null && 
            !await _stationChainRepository.ExistsAllById(filterDto.StationChainsIds))
        {
            validationErrors.Add("One or more of station chains ids is invalid");
        }

        if (filterDto.ServicesIds is not null &&
            !await _fuelStationServiceRepository.ExistsAllById(filterDto.ServicesIds))
        {
            validationErrors.Add("One or more of fuel station services ids is invalid");
        }

        if (validationErrors.Any())
        {
            throw new FilterValidationException(validationErrors);
        }
    }
    
    private IEnumerable<FuelStation> ApplyPriceFilter(IEnumerable<FuelStation> fuelStations, FuelStationFilterDto filter)
    {
        var minPrice = filter.MinPrice;
        var maxPrice = filter.MaxPrice;

        return fuelStations.Where(fs =>
            (minPrice == null || minPrice <= fs.FuelPrices.First().Price) &&
            (maxPrice == null || maxPrice >= fs.FuelPrices.First().Price));
    }
}