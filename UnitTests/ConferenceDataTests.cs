using API.Models;

namespace UnitTests;

public class ConferenceDataModelTests
{
    [Fact]
    public void Test_AllConferenceDataIsLoaded()
    {
        var conferenceData = ConferenceData.SpeakerSummary();
        Assert.NotNull(conferenceData);
        Assert.NotEmpty(conferenceData);
    }
}