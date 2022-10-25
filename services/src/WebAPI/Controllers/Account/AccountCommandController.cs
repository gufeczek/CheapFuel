using Application.Models;
using Application.Users.Commands.AuthenticateUser;
using Application.Users.Commands.ChangeUserRole;
using Application.Users.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.Account;

[ApiController]
[Route("api/v1/accounts")]
public class AccountCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterAsync([FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginAsync([FromBody] AuthenticateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [AuthorizeAdmin]
    [HttpPost("change-role")]
    public async Task<ActionResult> ChangeRoleAsync([FromBody] ChangeUserRoleCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}