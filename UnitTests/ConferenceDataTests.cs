using System.Text;
using System.Text.Json;
using API.Models;

namespace UnitTests;

public class ConferenceDataModelTests
{
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
                "name": "RubyConf",
                "location": "Melbourne, Australia",
                "year": "2013",
                "totalSpeakers": 27,
                "numberOfWomen": 2,
                "source": "https://rubyconf.org.au/2013/speakers",
                "dateAdded": "2018-10-01",
                "confDate": "2013-02-22",
                "Notes": "Not including lightning talks"
            },
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

        ]
        """;

    private static SpeakerSummary BaseConf()
    {
        return new SpeakerSummary
        {
            Name = "DevOps Days",
            Location = "Newcastle, Australia",
            Year = "2018",
            TotalSpeakers = 14,
            NumberOfWomen = 3,
            Source = "https://devopsdaysnewy.org/speakers/",
            DateAdded = new DateOnly(2018, 10, 25),
            ConfDate = new DateOnly(2018, 10, 24),
        };
    }

    private static string ConfWith(int totalSpeakers = 14, int numberOfWomen = 3)
    {
        var data = BaseConf();
        data.TotalSpeakers = totalSpeakers;
        data.NumberOfWomen = numberOfWomen;
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        return JsonSerializer.Serialize(data, options);
    }

    private static string ConfAddedOn(DateOnly dateAdded)
    {
        var data = BaseConf();
        data.DateAdded = dateAdded;

        return JsonSerializer.Serialize(data);
    }

    [Fact]
    public void Test_SingleConferenceDataIsLoaded()
    {
        var stubConfData = ConfStreamFrom($"[{ConfWith()}]");

        var conferenceData = new ConferenceData(stubConfData).SpeakerSummary();

        AssertSingleConfFields(conferenceData[0]);
    }
    [Fact]
    public void Test_MultipleConferencesAreLoaded()
    {
        var stubConfData = ConfStreamFrom($"[{ConfWith()}, {ConfWith()}]");

        var conferenceData = new ConferenceData(stubConfData).SpeakerSummary();
        
        Assert.Equal(2, conferenceData.Count);
        Assert.NotEqual(conferenceData[0], conferenceData[1]);
    }
    
    [Fact]
    public void Test_UnexpectedFieldsAreIgnored()
    {
        var stubConfData = ConfStreamFrom($"[{SingleConferenceWithExtraFields}]");

        var conferenceData = new ConferenceData(stubConfData).SpeakerSummary();

        AssertSingleConfFields(conferenceData[0]);
    }

    [Fact]
    public void Test_DerivedFieldsAreCalculatedCorrectly()
    {
        var stubConfData = ConfStreamFrom($"[{ConfWith(27, 2)}, {ConfWith(14, 3)}]");

        var conferenceData = new ConferenceData(stubConfData).SpeakerSummary();
        
        AssertDiversityPercentageCalc(conferenceData[0]);
        AssertDiversityPercentageCalc(conferenceData[1]);
        AssertNumberOfMenCalc(conferenceData[0]);
        AssertNumberOfMenCalc(conferenceData[1]);
    }

    [Fact]
    public void Test_MostRecentAddedConferenceIsAvailable()
    {
        var stubConfData = ConfStreamFrom($"[{ConfAddedOn(new DateOnly(2018, 10, 25))}, {ConfAddedOn(new DateOnly(2018, 10, 24))}]");
 
        var mostRecentlyAdded = new ConferenceData(stubConfData).MostRecentlyAdded();
        
        Assert.Equal(new DateOnly(2018, 10, 25), mostRecentlyAdded.DateAdded);
    }

    private static MemoryStream ConfStreamFrom(string confData)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(confData));
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
