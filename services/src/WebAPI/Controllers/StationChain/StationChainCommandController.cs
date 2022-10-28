using Application.Models;
using Application.StationChains.Commands.CreateStationChain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.StationChain;

[ApiController]
[AuthorizeAdmin]
[Route("api/v1/station-chains")]
public class StationChainCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public StationChainCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<StationChainDto>> CreateStationChain([FromBody] CreateStationChainCommand command)
    {
        var result = await _mediator.Send(command);
        return Created($"api/v1/station-chain/{result.Id}" ,result);
    }
}