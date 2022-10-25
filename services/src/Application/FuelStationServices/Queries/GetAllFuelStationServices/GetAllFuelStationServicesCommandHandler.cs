using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStationServices.Queries.GetAllFuelStationServices;

public class GetAllFuelStationServicesCommandHandler : IRequestHandler<GetAllFuelStationServicesCommand, IEnumerable<FuelStationServiceDto>>
{
    private readonly IFuelStationServiceRepository _serviceRepository;
    private readonly IMapper _mapper;

    public GetAllFuelStationServicesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _serviceRepository = unitOfWork.Services;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<FuelStationServiceDto>> Handle(GetAllFuelStationServicesCommand request, CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<FuelStationServiceDto>>(services);
    }
}