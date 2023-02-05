using Application.BlockUser.Commands.BlockUser;
using Application.BlockUser.Commands.UnblockUser;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers.BlockUser;

[ApiController]
[AuthorizeUser]
[Route("api/v1/blocked-users")]
public sealed class BlockUserCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public BlockUserCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AuthorizeAdmin]
    [HttpPost]
    public async Task<ActionResult<BlockUserDto>> BlockUser([FromBody] BlockUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [AuthorizeAdmin]
    [HttpDelete("{userId}")]
    public async Task<ActionResult> UnBlockUser([FromRoute] string username)
    {
        await _mediator.Send(new UnblockUserCommand(username));
        return NoContent();
    }
}