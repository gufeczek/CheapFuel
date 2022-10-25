using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStationServices.Queries.GetAllFuelStationServices;

public class GetAllFuelStationServicesCommandHandler : IRequestHandler<GetAllFuelStationServicesCommand, IEnumerable<FuelStationService>>
{
    private readonly IFuelStationServiceRepository _serviceRepository;

    public GetAllFuelStationServicesCommandHandler(IUnitOfWork unitOfWork)
    {
        _serviceRepository = unitOfWork.Services;
    }
    
    public async Task<IEnumerable<FuelStationService>> Handle(GetAllFuelStationServicesCommand request, CancellationToken cancellationToken)
    {
        return await _serviceRepository.GetAllAsync();
    }
}