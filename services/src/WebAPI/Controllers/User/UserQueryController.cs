using Application.Models;
using Application.Models.Filters;
using Application.Models.Pagination;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetLoggedUser;
using Application.Users.Queries.GetUser;
using Domain.Common.Pagination.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.User;

[ApiController]
[Route("api/v1/users")]
public class UserQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{username}")]
    public async Task<ActionResult<UserDto>> GetInfoAboutUser([FromRoute] string username)
    {
        var userInfo = await _mediator.Send(new GetUserQuery(username));
        return Ok(userInfo);
    }
    
    [HttpPost]
    [AuthorizeAdmin]
    public async Task<ActionResult<Page<UserDetailsDto>>> GetAllAsync([FromBody] UserFilterDto filter, [FromQuery] PageRequestDto pageRequestDto)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(filter, pageRequestDto));
        return Ok(result);
    }
    
    [HttpGet]
    [AuthorizeOwner]
    [Route("logged-user-info")]
    public async Task<ActionResult<UserDetailsDto>> GetInfoAboutLoggedUser()
    {
        var userInfo = await _mediator.Send(new GetLoggedUserQuery());
        return Ok(userInfo);
    }
}


