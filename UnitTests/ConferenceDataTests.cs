using API.Models;

namespace UnitTests;

public class ConferenceDataModelTests
{
    [Fact]
    public void Test_AllConferenceDataIsLoaded()
    {
        var conferenceData = ConferenceData.SpeakerSummary();
        
        var speakerSummary = conferenceData[0];
        Assert.NotNull(speakerSummary.Name);
        Assert.NotNull(speakerSummary.Location);
        Assert.True(speakerSummary.Year >= 2015);
        Assert.True(speakerSummary.TotalSpeakers > 0);
        Assert.True(speakerSummary.NumberOfWomen >= 0);
        Assert.NotNull(speakerSummary.Source);
        Assert.NotNull(speakerSummary.DateAdded);
        Assert.NotNull(speakerSummary.ConfDate);
    }
}