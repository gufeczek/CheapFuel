using Application.Models;
using Application.Models.Pagination;
using Application.ReportedReviews.Queries.GetAllReportedReviews;
using Domain.Common.Pagination.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.ReportedReviews;

[ApiController]
[AuthorizeUser]
[Route("api/v1/reports")]
public class ReportedReviewsQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportedReviewsQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AuthorizeAdmin]
    [HttpGet]
    public async Task<ActionResult<Page<FuelStationReportedReviewDto>>> GetAllAsync([FromQuery] PageRequestDto pageRequestDto)
    {
        var result = await _mediator.Send(new GetAllReportedReviewsQuery(pageRequestDto));
        return Ok(result);
    }
}