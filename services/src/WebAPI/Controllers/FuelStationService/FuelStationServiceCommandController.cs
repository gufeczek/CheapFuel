using Application.FuelStationServices.Commands.CreateFuelStationService;
using Application.FuelStationServices.Commands.DeleteFuelStationService;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.FuelStationService;

[ApiController]
[AuthorizeAdmin]
[Route("api/v1/services")]
public sealed class FuelStationServiceCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelStationServiceCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<ActionResult<FuelStationServiceDto>> CreateService([FromBody] CreateFuelStationServiceCommand command)
    {
        var result = await _mediator.Send(command);
        return Created($"api/v1/services/{result.Id}",result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteService([FromRoute] long id)
    {
        await _mediator.Send(new DeleteFuelStationServiceCommand(id));
        return NoContent();
    }
}