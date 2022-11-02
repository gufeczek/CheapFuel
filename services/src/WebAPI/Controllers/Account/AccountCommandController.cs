using Application.Models;
using Application.Users.Commands.AuthenticateUser;
using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.ChangeUserRole;
using Application.Users.Commands.GenerateEmailVerificationToken;
using Application.Users.Commands.RegisterUser;
using Application.Users.Commands.VerifyEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("verify/{token}")]
    public async Task<ActionResult> VerifyEmailAddress([FromRoute] string token)
    {
        await _mediator.Send(new VerifyEmailCommand(token));
        return Ok();
    }

    [Authorize]
    [HttpGet("generate-verification-token")]
    public async Task<ActionResult> GenerateEmailAddressVerificationToken()
    {
        await _mediator.Send(new GenerateEmailVerificationTokenCommand());
        return Ok();
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}