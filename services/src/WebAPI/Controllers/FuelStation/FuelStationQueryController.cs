using Application.FuelStations.Queries.GetAllFuelStationForMap;
using Application.FuelStations.Queries.GetFuelStationDetails;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.FuelStation;

[ApiController]
[Route("api/v1/fuel-stations")]
public sealed class FuelStationQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelStationQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [AuthorizeUser]
    [HttpGet("{id}")]
    public async Task<ActionResult<FuelStationDetailsDto>> GetFuelStationDetailsById([FromRoute] long id)
    {
        var result = await _mediator.Send(new GetFuelStationDetailsQuery(id));
        return Ok(result);
    }

    [AuthorizeUser]
    [HttpPost]
    public async Task<ActionResult<IEnumerable<SimpleMapFuelStationDto>>> GetAllFuelStationForMapView([FromBody] FuelStationFilterDto filterDto)
    {
        var result = await _mediator.Send(new GetAllFuelStationsForMapQuery(filterDto));
        return Ok(result);
    }
}