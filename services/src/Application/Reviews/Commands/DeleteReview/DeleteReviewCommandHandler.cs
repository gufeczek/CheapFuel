using Application.Common.Authentication;
using Application.Common.Exceptions;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Reviews.Commands.DeleteReview;

public sealed class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserPrincipalService _userPrincipalService;
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;

    public DeleteReviewCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _reviewRepository = unitOfWork.Reviews;
        _userPrincipalService = userPrincipalService;
    }
    
    public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
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
                $"User with username {user.Username} does not have permission to delete this review");
        }
        
        _reviewRepository.Remove(review);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}