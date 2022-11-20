using Application.Models;
using MediatR;

namespace Application.Reviews.Commands.CreateReview;

public sealed record CreateReviewCommand(
        int? Rate, 
        string? Content, 
        long? FuelStationId) : IRequest<FuelStationReviewDto>;
