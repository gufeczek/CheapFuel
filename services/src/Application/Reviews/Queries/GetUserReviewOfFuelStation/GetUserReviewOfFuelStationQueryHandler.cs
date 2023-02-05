using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Reviews.Queries.GetUserReviewOfFuelStation;

public sealed class GetUserReviewOfFuelStationQueryHandler : IRequestHandler<GetUserReviewOfFuelStationQuery, FuelStationReviewDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetUserReviewOfFuelStationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = unitOfWork.Users;
        _fuelStationRepository = unitOfWork.FuelStations;
        _reviewRepository = unitOfWork.Reviews;
        _mapper = mapper;
    }
    
    public async Task<FuelStationReviewDto> Handle(GetUserReviewOfFuelStationQuery request, CancellationToken cancellationToken)
    {
        if (!await _userRepository.ExistsByUsername(request.Username!))
        {
            throw new NotFoundException($"User not found for username = {request.Username}");
        }

        if (!await _fuelStationRepository.ExistsById((long)request.FuelStationId!))
        {
            throw new NotFoundException($"Fuel station not found for id = {request.FuelStationId}");
        }

        var review = await _reviewRepository.GetByFuelStationAndUsername((long)request.FuelStationId!, request.Username!)
                     ?? throw new NotFoundException($"Review not found for username = {request.Username} and fuel station id = {request.FuelStationId}");

        return _mapper.Map<FuelStationReviewDto>(review);
    }
}