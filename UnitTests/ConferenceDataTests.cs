using System.Diagnostics.CodeAnalysis;
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

    private static SpeakerSummary BaseConf(int totalSpeakers = 14, int numberOfWomen = 3)
    {
        return new SpeakerSummary
        {
            Name = "DevOps Days",
            Location = "Newcastle, Australia",
            Year = "2018",
            TotalSpeakers = totalSpeakers,
            NumberOfWomen = numberOfWomen,
            Source = "https://devopsdaysnewy.org/speakers/",
            DateAdded = new DateOnly(2018, 10, 25),
            ConfDate = new DateOnly(2018, 10, 24),
        };
    }

    private static string ConfWith(int totalSpeakers = 14, int numberOfWomen = 3)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        return JsonSerializer.Serialize(BaseConf(totalSpeakers, numberOfWomen), options);
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

        Assert.Equal(BaseConf(),conferenceData[0]);
    }
    [Fact]
    public void Test_MultipleConferencesAreLoaded()
    {
        var stubConfData = ConfStreamFrom($"[{ConfWith()}, {ConfWith(11, 4)}]");

        var conferenceData = new ConferenceData(stubConfData).SpeakerSummary();
        
        Assert.Equal(2, conferenceData.Count);
        Assert.NotEqual(conferenceData[0], conferenceData[1]);
    }
    
    [Fact]
    public void Test_UnexpectedFieldsAreIgnored()
    {
        var stubConfData = ConfStreamFrom($"[{SingleConferenceWithExtraFields}]");

        var conferenceData = new ConferenceData(stubConfData).SpeakerSummary();

        Assert.Equal(BaseConf(),conferenceData[0]);
    }

    [Theory]
    [InlineData(27, 2)]
    [InlineData(9, 0)]
    [InlineData(14, 3)]
    public void Test_DerivedFieldsAreCalculatedCorrectly(int totalSpeakers, int numberOfWomen)
    {
        var stubConfData = ConfStreamFrom($"[{ConfWith(totalSpeakers, numberOfWomen)}]");

        var conferenceData = new ConferenceData(stubConfData).SpeakerSummary()[0];
        
        Assert.Equal(conferenceData.NumberOfWomen / (float)conferenceData.TotalSpeakers,
            conferenceData.DiversityPercentage);
        Assert.Equal(conferenceData.TotalSpeakers - conferenceData.NumberOfWomen, conferenceData.NumberOfMen);
    }

    [Fact]
    public void Test_MostRecentAddedConferenceIsAvailable()
    {
        var mostRecentConfData = new DateOnly(2018, 10, 25);

        var confs = new List<string>()
        {
            ConfAddedOn(mostRecentConfData.AddDays(-1)), 
            ConfAddedOn(mostRecentConfData.AddMonths(-1)), 
            ConfAddedOn(mostRecentConfData)
        };
        var shuffledConfs = confs.OrderBy(conf => Guid.NewGuid()).ToList();
        var confData = ConfStreamFrom($"[{shuffledConfs[0]}, {shuffledConfs[1]}, {shuffledConfs[2]}]");
 
        var mostRecentlyAdded = new ConferenceData(confData).MostRecentlyAdded();
        
        Assert.Equal(mostRecentConfData, mostRecentlyAdded.DateAdded);
    }

    private static MemoryStream ConfStreamFrom(string confData)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(confData));
    }
}
