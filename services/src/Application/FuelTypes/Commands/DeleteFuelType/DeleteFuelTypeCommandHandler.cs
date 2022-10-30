using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelTypes.Commands.DeleteFuelType;

public sealed class DeleteFuelTypeCommandHandler : IRequestHandler<DeleteFuelTypeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelTypeRepository _fuelTypeRepository;

    public DeleteFuelTypeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _fuelTypeRepository = unitOfWork.FuelTypes;
    }
    
    public async Task<Unit> Handle(DeleteFuelTypeCommand request, CancellationToken cancellationToken)
    {
        var fuelType = await _fuelTypeRepository.GetAsync(request.Id.OrElseThrow()) 
                       ?? throw new NotFoundException($"Fuel type not found for id = {request.Id}");
        
        _fuelTypeRepository.Remove(fuelType);
        await _unitOfWork.SaveAsync();
            
        return Unit.Value;
    }
}