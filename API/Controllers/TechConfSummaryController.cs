using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace xxtechconfspeakers_api.Controllers;

[ApiController]
[Route("[controller]")]
public class TechConfSummaryController : ControllerBase
{
    // ReSharper disable once NotAccessedField.Local
    private readonly ILogger<TechConfSummaryController> _logger;
    private readonly ConferenceData _conferenceData;

    public TechConfSummaryController(ILogger<TechConfSummaryController> logger, IWebHostEnvironment hostingEnvironment)
    {
        _logger = logger;
        
        var dataPath = Path.Combine(hostingEnvironment.ContentRootPath, "Data");
        _conferenceData = new ConferenceData(dataPath);
    }

    [HttpGet(Name = "GetSummary")]
    public double Get()
    {
        return _conferenceData.AverageDiversityPercentage();
    }
}
