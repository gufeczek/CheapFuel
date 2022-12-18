using Application.FuelStations.Queries.GetAllFuelStationForList;
using Application.FuelStations.Queries.GetAllFuelStationForMap;
using Application.FuelStations.Queries.GetFuelStationDetails;
using Application.Models;
using Application.Models.Filters;
using Domain.Common.Pagination.Response;
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
    [HttpPost("map")]
    public async Task<ActionResult<IEnumerable<SimpleFuelStationDto>>> GetAllFuelStationForMapView([FromBody] FuelStationFilterDto filterDto)
    {
        var result = await _mediator.Send(new GetAllFuelStationsForMapQuery(filterDto));
        return Ok(result);
    }

    [AuthorizeUser]
    [HttpPost("list")]
    public async Task<ActionResult<Page<SimpleFuelStationDto>>> GetAllFuelStationForListView([FromBody] GetAllFuelStationForListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}