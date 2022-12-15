using Application.FuelAtStations.Commands.AddFuelToStation;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.FuelAtStation;

[ApiController]
[Route("api/v1/fuel-at-stations")]
public sealed class FuelAtStationCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelAtStationCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AuthorizeOwner]
    [HttpPost]
    public async Task<ActionResult<FuelAtStationDto>> CreateFuelAtStation([FromBody] AddFuelToStationCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result); // Should be updated after creating endpoint ot get fuel at station
    }
}