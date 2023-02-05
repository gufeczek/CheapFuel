using Application.FuelTypes.Commands.CreateFuelType;
using Application.FuelTypes.Commands.DeleteFuelType;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.FuelType;

[ApiController]
[AuthorizeAdmin]
[Route("api/v1/fuel-types")]
public sealed class FuelTypeCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelTypeCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<FuelTypeDto>> CreateFuelType([FromBody] CreateFuelTypeCommand command)
    {
        var result = await _mediator.Send(command);
        return Created($"api/v1/fuel-types/{result.Id}",result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFuelType([FromRoute] long id)
    {
        await _mediator.Send(new DeleteFuelTypeCommand(id));
        return NoContent();
    }
}