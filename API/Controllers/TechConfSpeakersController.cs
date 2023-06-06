using Microsoft.AspNetCore.Mvc;

namespace xxtechconfspeakers_api.Controllers;

[ApiController]
[Route("[controller]")]
public class TechConfSpeakersController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<TechConfSpeakersController> _logger;

    public TechConfSpeakersController(ILogger<TechConfSpeakersController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Get")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
