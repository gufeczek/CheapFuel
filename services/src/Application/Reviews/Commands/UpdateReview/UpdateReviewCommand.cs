using Application.Models;
using MediatR;

namespace Application.Reviews.Commands.UpdateReview;

public sealed record UpdateReviewCommand(
    long? ReviewId, 
    UpdateReviewDto? ReviewDto) : IRequest<FuelStationReviewDto>;