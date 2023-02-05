using Application.Models;
using Application.Models.Pagination;
using Application.StationChains.Queries.GetAllStationChains;
using Domain.Common.Pagination.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.StationChain;

[ApiController]
[Route("api/v1/station-chains")]
public sealed class StationChainQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public StationChainQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<Page<StationChainDto>>> GetAllAsync([FromQuery] PageRequestDto pageRequestDto)
    {
        var result = await _mediator.Send(new GetAllStationChainsQuery(pageRequestDto));
        return Ok(result);
    }
}