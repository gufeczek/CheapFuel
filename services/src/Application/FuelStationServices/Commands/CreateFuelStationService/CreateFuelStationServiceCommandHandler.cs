using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStationServices.Commands.CreateFuelStationService;

public class CreateFuelStationServiceCommandHandler : IRequestHandler<CreateFuelStationServiceCommand, Domain.Entities.FuelStationService>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationServiceRepository _serviceRepository;

    public CreateFuelStationServiceCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _serviceRepository = unitOfWork.Services;
    }
    
    public async Task<FuelStationService> Handle(CreateFuelStationServiceCommand request, CancellationToken cancellationToken)
    {
        var service = new FuelStationService
        {
            Name = request.Name
        };
        
        _serviceRepository.Add(service);
        await _unitOfWork.SaveAsync();

        return service;
    }
}