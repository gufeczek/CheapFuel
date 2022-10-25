using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelTypes.Commands.CreateFuelType;

public sealed class CreateFuelTypeCommandHandler : IRequestHandler<CreateFuelTypeCommand, FuelTypeDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IMapper _mapper;

    public CreateFuelTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _fuelTypeRepository = unitOfWork.FuelTypes;
        _mapper = mapper;
    }
    
    public async Task<FuelTypeDto> Handle(CreateFuelTypeCommand request, CancellationToken cancellationToken)
    {
        var fuelType = new FuelType
        {
            Name = request.Name
        };
        
        _fuelTypeRepository.Add(fuelType);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<FuelTypeDto>(fuelType);
    }
}