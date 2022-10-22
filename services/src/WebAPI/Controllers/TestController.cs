using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Authorization;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [Authorize]
    [HttpGet("user")]
    public string GetUser()
    {
        return "Hello world user!";
    }

    [AuthorizeAdmin]
    [HttpGet("admin")]
    public string GetAdmin()
    {
        return "Hello world admin!";
    }

    [AuthorizeOwner]
    [HttpGet("Owner")]
    public string GetOwner()
    {
        return "Hello world owner!";
    }
}