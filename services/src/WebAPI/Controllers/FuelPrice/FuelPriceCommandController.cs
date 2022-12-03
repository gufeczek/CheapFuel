using Application.FuelPrices.Commands.UpdateFuelPriceByOwner;
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
    [HttpPost]
    public async Task<ActionResult<IEnumerable<FuelPriceDto>>> UpdateFuelPrices([FromBody] NewFuelPricesAtStationDto dto)
    {
        var result = await _mediator.Send(new UpdateFuelPriceByOwnerCommand(dto));
        return Ok(result); // Should be updated after creating endpoint ot get fuel prices
    }
}