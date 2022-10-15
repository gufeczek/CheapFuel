using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly AppDbContext _context;
    
    public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpPost]
    public async Task<User> CreateUser([FromBody] User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    
    [HttpPut("/{id}")]
    public async Task<User> UpdateUser([FromRoute] long id, [FromBody] User user)
    {
        Console.WriteLine(id);
        User user1 = await _context.Users
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException("Test");

        user1.EmailConfirmed = !user1.EmailConfirmed;
        
        await _context.SaveChangesAsync();
        return user1;
    }
}