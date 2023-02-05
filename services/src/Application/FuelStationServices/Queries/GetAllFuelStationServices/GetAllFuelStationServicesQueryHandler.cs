using Application.Common;
using Application.Models;
using AutoMapper;
using Domain.Common.Pagination.Response;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStationServices.Queries.GetAllFuelStationServices;

public sealed class GetAllFuelStationServicesQueryHandler : IRequestHandler<GetAllFuelStationServicesQuery, Page<FuelStationServiceDto>>
{
    private readonly IFuelStationServiceRepository _serviceRepository;
    private readonly IMapper _mapper;

    public GetAllFuelStationServicesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _serviceRepository = unitOfWork.Services;
        _mapper = mapper;
    }
    
    public async Task<Page<FuelStationServiceDto>> Handle(GetAllFuelStationServicesQuery request, CancellationToken cancellationToken)
    {
        var pageRequest = PaginationHelper.Eval(request.PageRequestDto, new FuelStationServiceDtoColumnSelector());
        var result = await _serviceRepository.GetAllAsync(pageRequest);
        return Page<FuelStationServiceDto>.From(result, _mapper.Map<IEnumerable<FuelStationServiceDto>>(result.Data));
    }
}