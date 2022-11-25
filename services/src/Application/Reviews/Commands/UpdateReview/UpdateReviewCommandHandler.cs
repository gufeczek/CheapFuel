using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Reviews.Commands.UpdateReview;

public sealed class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, FuelStationReviewDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserPrincipalService _userPrincipalService;
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public UpdateReviewCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _reviewRepository = unitOfWork.Reviews;
        _userPrincipalService = userPrincipalService;
        _mapper = mapper;
    }
    
    public async Task<FuelStationReviewDto> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetAsync((long)request.ReviewId!) 
                     ?? throw new NotFoundException($"Review not found for id = {request.ReviewId}");
        var userId = _userPrincipalService.GetUserPrincipalId()
                     ?? throw new UnauthorizedException("User is not logged in!");
        var user = await _userRepository.GetAsync(userId)
                   ?? throw new NotFoundException($"User not found for id = {userId}");

        if (user.Role != Role.Admin && review.UserId != userId)
        {
            throw new ForbiddenException(
                $"User with username {user.Username} does not have permission to edit this review");
        }

        review.Content = request.ReviewDto!.Content;
        review.Rate = request.ReviewDto!.Rate;
        review.User = user;
        
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<FuelStationReviewDto>(review);
    }
}