using Application.Models;
using Application.ReportedReviews.Commands.CreateReportReview;
using Application.ReportedReviews.Commands.DeleteReportedReview;
using Application.ReportedReviews.Commands.UpdateReportedReview;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.ReportedReviews;

[ApiController]
[AuthorizeUser]
[Route("api/v1/reports")]
public sealed class ReportedReviewsCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportedReviewsCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CreateFuelStationReportedReviewDto>> CreateFuelStationReportedReview([FromBody] CreateReportReviewCommand command)
    {
        var result = await _mediator.Send(command);
        return Created($"api/v1/reports/{result}", result);
    }

    [AuthorizeAdmin]
    [HttpPut("{userId}/{reviewId}")]
    public async Task<ActionResult<UpdateReportedReviewDto>> UpdateFuelStationReportedReviewStatus([FromRoute] long reviewId, [FromRoute] long userId, [FromBody] UpdateReportedReviewDto dto)
    {
        var result = await _mediator.Send(new UpdateReportedReviewCommand(reviewId, userId, dto));
        return Ok(result);
    }

    [AuthorizeAdmin]
    [HttpDelete("{userId}/{reviewId}")]
    public async Task<ActionResult> DeleteFuelStationReportedReview([FromRoute] long reviewId, [FromRoute] long userId)
    {
        await _mediator.Send(new DeleteReportReviewCommand(reviewId,userId));
        return NoContent();
    } 
}