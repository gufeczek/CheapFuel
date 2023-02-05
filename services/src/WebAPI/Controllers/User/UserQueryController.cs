using Application.Models;
using Application.Models.Filters;
using Application.Models.Pagination;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetLoggedUser;
using Application.Users.Queries.GetUser;
using Application.Users.Queries.GetUserForAdministration;
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

    [HttpGet("admin/{username}")]
    public async Task<ActionResult<UserDetailsDto>> GetUserForAdmin([FromRoute] string username)
    {
        var result = await _mediator.Send(new GetUserForAdministrationQuery(username));
        return Ok(result);
    }

    [AuthorizeAdmin]
    [HttpPost]
    public async Task<ActionResult<Page<UserDetailsDto>>> GetAllAsync([FromBody] UserFilterDto filter, [FromQuery] PageRequestDto pageRequestDto)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(filter, pageRequestDto));
        return Ok(result);
    }

    [AuthorizeUser]
    [HttpGet]
    [Route("logged-user")]
    public async Task<ActionResult<UserDetailsDto>> GetInfoAboutLoggedUser()
    {
        var userInfo = await _mediator.Send(new GetLoggedUserQuery());
        return Ok(userInfo);
    }
}
