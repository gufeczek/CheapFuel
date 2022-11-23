using Application.Models;
using Application.Reviews.Commands.CreateReview;
using Application.Reviews.Commands.DeleteReview;
using Application.Reviews.Commands.UpdateReview;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.Review;

[ApiController]
[AuthorizeUser]
[Route("api/v1/reviews")]
public sealed class ReviewCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<FuelStationReviewDto>> CreateFuelStationReview([FromBody] CreateReviewCommand command)
    {
        var result = await _mediator.Send(command);
        return Created($"api/v1/reviews/{result.Id}", result);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<FuelStationReviewDto>> UpdateFuelStationReview([FromRoute] long id, [FromBody] UpdateReviewDto dto)
    {
        var result = await _mediator.Send(new UpdateReviewCommand(id, dto));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFuelStationReview([FromRoute] long id)
    {
        await _mediator.Send(new DeleteReviewCommand(id));
        return NoContent();
    }
}