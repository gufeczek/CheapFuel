using Application.Favorites.Queries.GetFavourite;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.Favourite;

[ApiController]
[AuthorizeUser]
[Route("api/v1/favourites")]
public sealed class FavouriteQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavouriteQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{username}/{fuelStationId}")]
    public async Task<ActionResult<SimpleUserFavouriteDto>> GetFavourite([FromRoute] string username, [FromRoute] long fuelStationId)
    {
        var result = await _mediator.Send(new GetFavouriteQuery(username, fuelStationId));
        return Ok(result);
    }
}