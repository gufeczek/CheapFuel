using Application.Favorites.Commands.CreateFavorite;
using Application.Favorites.Commands.DeleteFavorite;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.Favourite;

[ApiController]
[AuthorizeUser]
[Route("api/v1/favourites")]
public sealed class FavouriteCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavouriteCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<SimpleUserFavouriteDto>> CreateFavourite([FromBody] CreateFavouriteCommand command)
    {
        var result = await _mediator.Send(command);
        return Created($"api/v1/favourites/${result.Username}/${result.FuelStationId}", result);
    }

    [HttpDelete("{fuelStationId}")]
    public async Task<IActionResult> DeleteFavourite([FromRoute] long fuelStationId)
    {
        await _mediator.Send(new DeleteFavoriteCommand(fuelStationId));
        return NoContent();
    }
}