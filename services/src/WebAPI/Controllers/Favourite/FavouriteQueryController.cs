using Application.Favorites.Queries.GetAllLoggedUserFavourite;
using Application.Favorites.Queries.GetFavourite;
using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
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

    [AuthorizeUser]
    [HttpGet]
    public async Task<ActionResult<Page<UserFavouriteDto>>> GetAllLoggedUserFavourites([FromQuery] PageRequestDto pageRequest)
    {
        var result = await _mediator.Send(new GetAllLoggedUserFavouriteQuery(pageRequest));
        return Ok(result);
    }
}