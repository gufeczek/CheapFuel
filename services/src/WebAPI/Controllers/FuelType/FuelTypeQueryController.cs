using Application.FuelTypes.Queries.GetAllFuelTypes;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.FuelType;

[ApiController]
[Route("api/v1/FuelTypes")]
public sealed class FuelTypeQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelTypeQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FuelTypeDto>>> GetAllFuelTypes()
    {
        var fuelTypes = await _mediator.Send(new GetAllFuelTypesQuery());
        return Ok(fuelTypes);
    }
}