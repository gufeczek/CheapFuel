using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelStationServices.Commands.DeleteFuelStationService;

public sealed class DeleteFuelStationServiceCommandHandler : IRequestHandler<DeleteFuelStationServiceCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelStationServiceRepository _serviceRepository;

    public DeleteFuelStationServiceCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _serviceRepository = unitOfWork.Services;
    }
    
    public async Task<Unit> Handle(DeleteFuelStationServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetAsync(request.Id.OrElseThrow()) 
                                     ?? throw new NotFoundException($"Service not found for id = {request.Id}");
        
        _serviceRepository.Remove(service);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}