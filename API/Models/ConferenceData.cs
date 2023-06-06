using System.Text.Json;

namespace API.Models
{
    public class ConferenceData
    {
        public ConferenceData(Stream dataSource)
        {
            var confData = new StreamReader(dataSource).ReadToEnd();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            ConfDetails = JsonSerializer.Deserialize<SpeakerSummary>(confData, options);
        }

        private SpeakerSummary? ConfDetails { get; init; }

        public List<SpeakerSummary> SpeakerSummary()
        {
            return new List<SpeakerSummary>() { ConfDetails! };
        }
    }

    public class SpeakerSummary
    {
        public string Name { get; init; }
        public string Location { get; init; }
        public string Year { get; init; }
        public int TotalSpeakers { get; init;}
        public int NumberOfWomen { get; init;}
        public string Source { get; init;}
        public DateOnly DateAdded { get; init;}
        public DateOnly ConfDate { get; init; }
    }
}