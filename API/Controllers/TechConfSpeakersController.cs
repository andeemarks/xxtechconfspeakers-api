using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace xxtechconfspeakers_api.Controllers;

[ApiController]
[Route("[controller]")]
public class TechConfSpeakersController : ControllerBase
{
    // ReSharper disable once NotAccessedField.Local
    private readonly ILogger<TechConfSpeakersController> _logger;
    private readonly ConferenceData _conferenceData;

    public TechConfSpeakersController(ILogger<TechConfSpeakersController> logger, IWebHostEnvironment hostingEnvironment)
    {
        _logger = logger;
        
        var dataPath = Path.Combine(hostingEnvironment.ContentRootPath, "Data");
        _conferenceData = new ConferenceData(dataPath);
    }

    [HttpGet(Name = "Get")]
    public IEnumerable<SpeakerSummary> Get()
    {
        return _conferenceData.SpeakerSummary();
    }
}
