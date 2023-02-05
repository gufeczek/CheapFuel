using Application.Models;
using Application.ServiceAtStations.Commands.AddServiceToStation;
using Application.ServiceAtStations.Commands.RemoveServiceFromStation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.ServiceAtStation;

[ApiController]
[Route("api/v1/service-at-station")]
public class ServiceAtStationCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceAtStationCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AuthorizeOwner]
    [HttpPost]
    public async Task<ActionResult<ServiceAtStationDto>> CreateServiceAtStation([FromBody] AddServiceToStationCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result); // Should be updated after creating endpoint ot get service at station
    }

    [AuthorizeOwner]
    [HttpDelete("{fuelStationId}/{serviceId}")]
    public async Task<ActionResult> DeleteServiceAtStation([FromRoute] long fuelStationId, [FromRoute] long serviceId)
    {
        await _mediator.Send(new RemoveServiceFromStationCommand(fuelStationId, serviceId));
        return NoContent();
    }
}