using Application.Users.Commands.AuthenticateUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/v1/accounts")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] AuthenticateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}