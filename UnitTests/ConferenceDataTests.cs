using System.Text;
using API.Models;

namespace UnitTests;

public class ConferenceDataModelTests
{
    private const string SingleConference = """
        {
            "name": "DevOps Days",
            "location": "Newcastle, Australia",
            "year": "2018",
            "totalSpeakers": 14,
            "numberOfWomen": 3,
            "source": "https://devopsdaysnewy.org/speakers/",
            "dateAdded": "2018-10-25",
            "confDate": "2018-10-24"
          }
        """;

    private const string SingleConferenceWithExtraFields = """
        {
            "name": "DevOps Days",
            "location": "Newcastle, Australia",
            "year": "2018",
            "totalSpeakers": 14,
            "numberOfWomen": 3,
            "source": "https://devopsdaysnewy.org/speakers/",
            "dateAdded": "2018-10-25",
            "confDate": "2018-10-24",
            "lunchLineMaxLength": 100
          }
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
        var stubConfData = new MemoryStream(Encoding.UTF8.GetBytes($"[{SingleConference}]"));

        var conferenceData = new ConferenceData(stubConfData).SpeakerSummary();

        AssertSingleConfFields(conferenceData[0]);
    }
    [Fact]
    public void Test_MultipleConferencesAreLoaded()
    {
        var stubConfData = new MemoryStream(Encoding.UTF8.GetBytes(MultipleConferences));

        var conferenceData = new ConferenceData(stubConfData).SpeakerSummary();
        
        Assert.Equal(2, conferenceData.Count);
        Assert.NotEqual(conferenceData[0], conferenceData[1]);
    }
    
    [Fact]
    public void Test_UnexpectedFieldsAreIgnored()
    {
        var stubConfData = new MemoryStream(Encoding.UTF8.GetBytes($"[{SingleConferenceWithExtraFields}]"));

        var conferenceData = new ConferenceData(stubConfData).SpeakerSummary();

        AssertSingleConfFields(conferenceData[0]);
    }

    [Fact]
    public void Test_DerivedFieldsAreCalculatedCorrectly()
    {
        var stubConfData = new MemoryStream(Encoding.UTF8.GetBytes(MultipleConferences));

        var conferenceData = new ConferenceData(stubConfData).SpeakerSummary();
        
        AssertDiversityPercentageCalc(conferenceData[0]);
        AssertDiversityPercentageCalc(conferenceData[1]);
        AssertNumberOfMenCalc(conferenceData[0]);
        AssertNumberOfMenCalc(conferenceData[1]);
    }

    private static void AssertSingleConfFields(SpeakerSummary speakerSummary)
    {
        Assert.Equal("DevOps Days", speakerSummary.Name);
        Assert.Equal("Newcastle, Australia", speakerSummary.Location);
        Assert.Equal("2018", speakerSummary.Year);
        Assert.Equal(14, speakerSummary.TotalSpeakers);
        Assert.Equal(3, speakerSummary.NumberOfWomen);
        Assert.NotNull(speakerSummary.Source);
        Assert.Equal(new DateOnly(2018, 10, 25), speakerSummary.DateAdded);
        Assert.Equal(new DateOnly(2018, 10, 24), speakerSummary.ConfDate);
    }

    private static void AssertNumberOfMenCalc(SpeakerSummary conferenceData)
    {
        Assert.Equal(conferenceData.TotalSpeakers - conferenceData.NumberOfWomen, conferenceData.NumberOfMen);
    }

    private static void AssertDiversityPercentageCalc(SpeakerSummary conferenceData)
    {
        Assert.Equal(conferenceData.NumberOfWomen / (float)conferenceData.TotalSpeakers,
            conferenceData.DiversityPercentage);
    }
}