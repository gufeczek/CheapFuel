using Application.FuelStationServices.Queries.GetAllFuelStationServices;
using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.FuelStationService;

[ApiController]
[Route("api/v1/services")]
public sealed class FuelStationServiceQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelStationServiceQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<Page<FuelStationServiceDto>>> GetAllAsync([FromQuery] PageRequestDto pageRequestDto)
    {
        var result = await _mediator.Send(new GetAllFuelStationServicesQuery(pageRequestDto));
        return Ok(result);
    }
}