using Application.FuelPrices.Commands.UpdateFuelPriceByOwner;
using Application.FuelPrices.Commands.UpdateFuelPriceByUser;
using Application.Models.FuelPriceDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.FuelPrice;

[ApiController]
[Route("api/v1/fuel-prices")]
public sealed class FuelPriceCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelPriceCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AuthorizeOwner]
    [HttpPost("owner")]
    public async Task<ActionResult<IEnumerable<FuelPriceDto>>> UpdateFuelPricesByOwner([FromBody] NewFuelPricesAtStationDto dto)
    {
        var result = await _mediator.Send(new UpdateFuelPriceByOwnerCommand(dto));
        return Ok(result);
    }
    
    [AuthorizeUser]
    [HttpPost("user")]
    public async Task<ActionResult<IEnumerable<FuelPriceDto>>> UpdateFuelPricesByUser([FromBody] NewFuelPricesAtStationWithLocation dto)
    {
        var result = await _mediator.Send(new UpdateFuelPriceByUserCommand(dto));
        return Ok(result);
    }
}