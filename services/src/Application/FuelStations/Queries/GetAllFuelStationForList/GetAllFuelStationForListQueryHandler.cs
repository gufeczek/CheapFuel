using Application.Common;
using Application.Common.Exceptions;
using Application.Models;
using Application.Models.Filters;
using AutoMapper;
using Domain.Common.Pagination.Response;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStations.Queries.GetAllFuelStationForList;

public class GetAllFuelStationForListQueryHandler 
    : IRequestHandler<GetAllFuelStationForListQuery, Page<SimpleFuelStationDto>>
{
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IFuelStationServiceRepository _fuelStationServiceRepository;
    private readonly IStationChainRepository _stationChainRepository;
    private readonly IMapper _mapper;
    
    public GetAllFuelStationForListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _fuelStationRepository = unitOfWork.FuelStations;
        _fuelTypeRepository = unitOfWork.FuelTypes;
        _fuelStationServiceRepository = unitOfWork.Services;
        _stationChainRepository = unitOfWork.StationChains;
        _mapper = mapper;
    }
    
    public async Task<Page<SimpleFuelStationDto>> Handle(GetAllFuelStationForListQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        await ValidateFilters(filter!);

        var pageRequest = PaginationHelper.Eval(request.PageRequest, new SimpleFuelStationDtoColumnSelector());
        var result = await _fuelStationRepository.GetFuelStationsWithPrices(
            filter!.FuelTypeId,
            filter.ServicesIds,
            filter.StationChainsIds,
            filter.MinPrice,
            filter.MaxPrice,
            pageRequest);

        return Page<SimpleFuelStationDto>.From(result, _mapper.Map<IEnumerable<SimpleFuelStationDto>>(result.Data));
    }
    
    private async Task ValidateFilters(FuelStationFilterWithLocalizationDto filterDto)
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
}