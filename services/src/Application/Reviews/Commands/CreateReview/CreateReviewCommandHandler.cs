using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, FuelStationReviewDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    private readonly IMapper _mapper;

    public CreateReviewCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _reviewRepository = unitOfWork.Reviews;
        _userPrincipalService = userPrincipalService;
        _mapper = mapper;
    }
    
    public async Task<FuelStationReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var username = _userPrincipalService.GetUserName()
                       ?? throw new UnauthorizedException("User is not logged in!");
        var user = await _userRepository.GetByUsernameAsync(username) 
                   ?? throw new NotFoundException($"User not found for username = {username}");

        if (await _reviewRepository.ExistsByFuelStationAndUsername((long)request.FuelStationId!, username))
        {
            throw new ConflictException(
                $"User with username = {username} has already left a review for fuel station with id = {request.FuelStationId}");
        }
        
        var review = new Review
        {
            Rate = request.Rate,
            Content = request.Content,
            FuelStationId = request.FuelStationId,
            User = user
        };
        
        _reviewRepository.Add(review);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<FuelStationReviewDto>(review);
    }
}