using Application.Models;
using MediatR;

namespace Application.Reviews.Queries.GetUserReviewOfFuelStation;

public sealed record GetUserReviewOfFuelStationQuery(string? Username, long? FuelStationId) 
    : IRequest<FuelStationReviewDto>;