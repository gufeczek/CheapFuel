using Application.Common.Exceptions;
using Application.Models;
using Application.Models.Filters;
using Application.Models.Pagination;
using AutoMapper;
using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using Domain.Entities;
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

        var result = await _fuelStationRepository.GetFuelStationsWithFuelPrice(
            filter!.FuelTypeId,
            filter.ServicesIds,
            filter.StationChainsIds,
            filter.MinPrice,
            filter.MaxPrice);

        if (request.PageRequest.Sort is not null)
        {
            result = OrderBy(result, request.PageRequest.Sort, request.Filter!);
        }

        return Paginate(result, request.PageRequest);
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

    private IEnumerable<FuelStation> OrderBy(
        IEnumerable<FuelStation> fuelStations, 
        SortDto sortDto, 
        FuelStationFilterWithLocalizationDto filter)
    {
        return sortDto.SortBy switch
        {
            "Distance" => OrderByDistance(fuelStations, sortDto, filter),
            "Price" => OrderByPrice(fuelStations, sortDto),
            "Updated" => OrderByLastUpdate(fuelStations, sortDto),
            _ => throw new BadRequestException("Wrong sort property")
        };
    }

    private IEnumerable<FuelStation> OrderByDistance(
        IEnumerable<FuelStation> fuelStations, 
        SortDto sortDto, 
        FuelStationFilterWithLocalizationDto filter)
    {
        if (filter.UserLongitude is null || filter.UserLatitude is null) 
            return fuelStations;

        double Func(FuelStation fs) => GetDistance(
            (double)fs.GeographicalCoordinates!.Longitude, 
            (double)fs.GeographicalCoordinates.Latitude, 
            (double)filter.UserLongitude, 
            (double)filter.UserLatitude);

        return OrderBy(fuelStations, Func, sortDto.SortDirection!.Value);
    }
    
    private double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
    {
        var d1 = latitude * (Math.PI / 180.0);
        var num1 = longitude * (Math.PI / 180.0);
        var d2 = otherLatitude * (Math.PI / 180.0);
        var num2 = otherLongitude * (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
    
        return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
    }

    private IEnumerable<FuelStation> OrderByPrice(IEnumerable<FuelStation> fuelStations, SortDto sortDto)
    {
        decimal Func(FuelStation fs) => fs.FuelPrices.First().Price;
        return OrderBy(fuelStations, Func, sortDto.SortDirection!.Value);
    }

    private IEnumerable<FuelStation> OrderByLastUpdate(IEnumerable<FuelStation> fuelStations, SortDto sortDto)
    {
        DateTime Func(FuelStation fs) => fs.FuelPrices.First().CreatedAt;
        return OrderBy(fuelStations, Func, sortDto.SortDirection!.Value);
    }

    private IEnumerable<FuelStation> OrderBy<T>(IEnumerable<FuelStation> fuelStations, Func<FuelStation, T> func, SortDirection direction)
    {
        return direction == SortDirection.Asc
            ? fuelStations.OrderBy(func)
            : fuelStations.OrderByDescending(func);
    }

    private Page<SimpleFuelStationDto> Paginate(IEnumerable<FuelStation> fuelStations, PageRequestDto pageRequest)
    {
        var fuelStationsArr = fuelStations as FuelStation[] ?? fuelStations.ToArray();
        
        var totalElements = fuelStationsArr.Count();
        var totalPages = (int)Math.Ceiling(totalElements / (decimal)pageRequest.PageSize!);

        var data = fuelStationsArr.Skip(pageRequest.PageSize.Value * (pageRequest.PageNumber!.Value - 1))
            .Take(pageRequest.PageSize.Value)
            .ToList();

        return new Page<SimpleFuelStationDto>
        {
            PageNumber = pageRequest.PageNumber.Value,
            PageSize = pageRequest.PageSize.Value,
            NextPage = pageRequest.PageNumber < totalPages ? pageRequest.PageNumber + 1 : null,
            PreviousPage = pageRequest.PageNumber > 1 ? pageRequest.PageNumber - 1 : null,
            FirstPage = 1,
            LastPage = totalPages,
            TotalPages = totalPages,
            TotalElements = totalElements,
            Sort = pageRequest.Sort is not null
                ? new Sort { SortBy = pageRequest.Sort.SortBy, Direction = pageRequest.Sort.SortDirection }
                : null,
            Data = _mapper.Map<IEnumerable<SimpleFuelStationDto>>(data)
        };
    }
}