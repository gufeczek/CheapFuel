using Application.Models;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStationServices.Queries.GetAllFuelStationServices;

public sealed class GetAllFuelStationServicesQueryHandler : IRequestHandler<GetAllFuelStationServicesQuery, IEnumerable<FuelStationServiceDto>>
{
    private readonly IFuelStationServiceRepository _serviceRepository;
    private readonly IMapper _mapper;

    public GetAllFuelStationServicesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _serviceRepository = unitOfWork.Services;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<FuelStationServiceDto>> Handle(GetAllFuelStationServicesQuery request, CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<FuelStationServiceDto>>(services);
    }
}