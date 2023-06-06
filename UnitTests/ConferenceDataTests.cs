using System.Text;
using API.Models;

namespace UnitTests;

public class ConferenceDataModelTests
{
    private const string SingleConference = """
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

    private const string MultipleConferences = """
        [
            {
                "name": "DevOps Days",
                "location": "Newcastle, Australia",
                "year": "2018",
                "totalSpeakers": 14,
                "numberOfWomen": 3,
                "source": "https://devopsdaysnewy.org/speakers/",
                "dateAdded": "2018-10-25",
                "confDate": "2018-10-24"
            },
            {
                "name": "RubyConf",
                "location": "Melbourne, Australia",
                "year": "2013",
                "totalSpeakers": 27,
                "numberOfWomen": 2,
                "source": "https://rubyconf.org.au/2013/speakers",
                "dateAdded": "2018-10-01",
                "confDate": "2013-02-22",
                "Notes": "Not including lightning talks"
            }
        ]
        """;

    [Fact]
    public void Test_SingleConferenceDataIsLoaded()
    {
        var stubConfData = new MemoryStream(Encoding.UTF8.GetBytes(SingleConference));
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

    [Fact]
    public void Test_DiversityPercentagesAreDerived()
    {
        var stubConfData = new MemoryStream(Encoding.UTF8.GetBytes(MultipleConferences));
        var model = new ConferenceData(stubConfData);
        
        var conferenceData = model.SpeakerSummary();
        
        Assert.Equal(0.2142857164144516, conferenceData[0].DiversityPercentage);
        Assert.Equal(0.074074074625968933, conferenceData[1].DiversityPercentage);
    }
    
    [Fact]
    public void Test_NumberOfMenAreDerived()
    {
        var stubConfData = new MemoryStream(Encoding.UTF8.GetBytes(MultipleConferences));
        var model = new ConferenceData(stubConfData);
        
        var conferenceData = model.SpeakerSummary();
        
        Assert.Equal(11, conferenceData[0].NumberOfMen);
        Assert.Equal(25, conferenceData[1].NumberOfMen);
    }
}