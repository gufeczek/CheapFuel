using Application.BlockUser.Queries.GetAllBlockedUsers;
using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.BlockUser;

[ApiController]
[AuthorizeUser]
[Route("api/v1/blocked-users")]
public class BlockUserQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public BlockUserQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AuthorizeAdmin]
    [HttpGet]
    public async Task<ActionResult<Page<AllBlockedUsersDto>>> GetAllAsync([FromQuery] PageRequestDto pageRequestDto)
    {
        var result = await _mediator.Send(new GetAllBlockedUsersQuery(pageRequestDto));
        return Ok(result);
    }
}