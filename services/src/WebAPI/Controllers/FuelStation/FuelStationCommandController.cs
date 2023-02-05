using Application.FuelStations.Commands.CreateFuelStation;
using Application.FuelStations.Commands.DeleteFuelStation;
using Application.Models;
using Application.Models.FuelStationDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.FuelStation;

[ApiController]
[Route("api/v1/fuel-stations")]
public sealed class FuelStationCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelStationCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AuthorizeAdmin]
    [HttpPost]
    public async Task<ActionResult<FuelStationDto>> CreateFuelStation([FromBody] NewFuelStationDto dto)
    {
        var result = await _mediator.Send(new CreateFuelStationCommand(dto));
        return Ok(result);
    }

    [AuthorizeAdmin]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFuelStation([FromRoute] long id)
    {
        await _mediator.Send(new DeleteFuelStationCommand(id));
        return NoContent();
    }
}