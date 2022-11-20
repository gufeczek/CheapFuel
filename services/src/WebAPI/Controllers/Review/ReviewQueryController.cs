using Application.Models;
using Application.Models.Pagination;
using Application.Reviews.Queries.GetAllFuelStationReviews;
using Domain.Common.Pagination.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Review;

[ApiController]
[Route("api/v1/reviews")]
public sealed class ReviewQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("fuel-station/{id}")]
    public async Task<ActionResult<Page<FuelStationReviewDto>>> GetAllFuelStationReviews([FromRoute] long id, [FromQuery] PageRequestDto pageRequestDto)
    {
        var reviews = await _mediator.Send(new GetAllFuelStationReviewsQuery(id, pageRequestDto));
        return Ok(reviews);
    }
}