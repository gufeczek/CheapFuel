using Application.FuelStations.Queries.GetAllFuelStationForMap;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.FuelStation;

[ApiController]
[Route("api/v1/fuel-stations")]
public class FuelStationQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelStationQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AuthorizeUser]
    [HttpPost]
    public async Task<ActionResult<IEnumerable<SimpleMapFuelStationDto>>> GetAllFuelStationForMapView([FromBody] FuelStationFilterDto filterDto)
    {
        var result = await _mediator.Send(new GetAllFuelStationsForMapQuery(filterDto));
        return Ok(result);
    }
}