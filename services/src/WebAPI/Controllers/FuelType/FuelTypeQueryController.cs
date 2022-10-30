using Application.FuelTypes.Queries.GetAllFuelTypes;
using Application.Models;
using Application.Models.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.FuelType;

[ApiController]
[Route("api/v1/fuel-types")]
public sealed class FuelTypeQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelTypeQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FuelTypeDto>>> GetAllFuelTypes([FromQuery] PageRequestDto pageRequestDto)
    {
        var fuelTypes = await _mediator.Send(new GetAllFuelTypesQuery(pageRequestDto));
        return Ok(fuelTypes);
    }
}