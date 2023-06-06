using System.Text;
using API.Models;

namespace UnitTests;

public class ConferenceDataModelTests
{
    private const string ConfData = """
        [{
            "name": "DevOps Days",
            "location": "Newcastle, Australia",
            "year": "2018",
            "totalSpeakers": 14,
            "numberOfWomen": 3,
            "source": "https://devopsdaysnewy.org/speakers/",
            "dateAdded": "2018-10-25",
            "confDate": "2018-10-24"
          }]
        """;

    [Fact]
    public void Test_AllConferenceDataIsLoaded()
    {
        var stubConfData = new MemoryStream(Encoding.UTF8.GetBytes(ConfData));
        var model = new ConferenceData(stubConfData);
        
        var conferenceData = model.SpeakerSummary();
        
        var speakerSummary = conferenceData[0];
        Assert.Equal("DevOps Days", speakerSummary.Name);
        Assert.Equal("Newcastle, Australia", speakerSummary.Location);
        Assert.Equal("2018", speakerSummary.Year);
        Assert.Equal(14, speakerSummary.TotalSpeakers);
        Assert.Equal(3, speakerSummary.NumberOfWomen);
        Assert.NotNull(speakerSummary.Source);
        Assert.Equal(new DateOnly(2018, 10, 25), speakerSummary.DateAdded);
        Assert.Equal(new DateOnly(2018, 10, 24), speakerSummary.ConfDate);
    }
}