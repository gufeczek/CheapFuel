using Application.Models.OwnedStations;
using Application.OwnerStations.Commands.CreateFuelStationOwner;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.OwnedStation;

[ApiController]
[Route("api/v1/owned-stations")]
public class OwnedStationCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public OwnedStationCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AuthorizeAdmin]
    [HttpPost]
    public async Task<ActionResult<OwnedStationDto>> CreateOwnedStation([FromBody] CreateFuelStationOwnerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}