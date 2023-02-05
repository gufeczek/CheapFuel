using Application.FuelStations.Queries.GetAllFuelStationForList;
using Application.FuelStations.Queries.GetAllFuelStationForMap;
using Application.FuelStations.Queries.GetFuelStationDetails;
using Application.FuelStations.Queries.GetMostEconomicalFuelStation;
using Application.Models;
using Application.Models.Filters;
using Application.Models.FuelStationDtos;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.FuelStation;

[ApiController]
[AuthorizeUser]
[Route("api/v1/fuel-stations")]
public sealed class FuelStationQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelStationQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<FuelStationDetailsDto>> GetFuelStationDetailsById([FromRoute] long id)
    {
        var result = await _mediator.Send(new GetFuelStationDetailsQuery(id));
        return Ok(result);
    }

    [HttpPost("map")]
    public async Task<ActionResult<IEnumerable<SimpleFuelStationDto>>> GetAllFuelStationForMapView([FromBody] FuelStationFilterDto filterDto)
    {
        var result = await _mediator.Send(new GetAllFuelStationsForMapQuery(filterDto));
        return Ok(result);
    }

    [HttpPost("list")]
    public async Task<ActionResult<Page<SimpleFuelStationDto>>> GetAllFuelStationForListView([FromBody] FuelStationFilterWithLocalizationDto filterDto, [FromQuery] PageRequestDto pageRequestDto)
    {
        var result = await _mediator.Send(new GetAllFuelStationForListQuery(filterDto, pageRequestDto));
        return Ok(result);
    }

    [HttpPost("most-economical")]
    public async Task<ActionResult<MostEconomicalFuelStationDto>> GetMostEconomicalFuelStation([FromBody] GetMostEconomicalFuelStationQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}