using Application.FuelStationServices.Commands.CreateFuelStationService;
using Application.FuelStationServices.Commands.DeleteService;
using Application.FuelStationServices.Queries.GetAllFuelStationServices;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/services")]
public class FuelStationServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelStationServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FuelStationServiceDto>>> GetAllAsync()
    {
        var result = await _mediator.Send(new GetAllFuelStationServicesCommand());
        return Ok(result);
    }

    [AuthorizeAdmin]
    [HttpPost]
    public async Task<ActionResult<FuelStationServiceDto>> CreateService([FromBody] CreateFuelStationServiceCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [AuthorizeAdmin]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteService([FromRoute] long id)
    {
        await _mediator.Send(new DeleteFuelStationServiceCommand(id));
        return NoContent();
    }
}