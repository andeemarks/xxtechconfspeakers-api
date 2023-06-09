using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace xxtechconfspeakers_api.Controllers;

[ApiController]
[Route("[controller]")]
public class TechConfSpeakersController : ControllerBase
{
    // ReSharper disable once NotAccessedField.Local
    private readonly ILogger<TechConfSpeakersController> _logger;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public TechConfSpeakersController(ILogger<TechConfSpeakersController> logger, IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
        _logger = logger;
    }

    [HttpGet(Name = "Get")]
    public IEnumerable<SpeakerSummary> Get()
    {
        var dataPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Data");
        
        return new ConferenceData(dataPath).SpeakerSummary();
    }
}
