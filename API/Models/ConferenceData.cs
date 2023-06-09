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
            ConfDetails = JsonSerializer.Deserialize<List<SpeakerSummary>>(confData, options);
        }

        public ConferenceData(string dataSourcePath) : this(new FileStream(dataSourcePath + "/confs.json", new FileStreamOptions())) { }

        private List<SpeakerSummary>? ConfDetails { get; }

        public List<SpeakerSummary> SpeakerSummary()
        {
            return ConfDetails!;
        }
    }

    public class SpeakerSummary
    {
        public string? Name { get; set; }
        public string? Location { get; init; }
        public string? Year { get; init; }
        public int TotalSpeakers { get; init;}
        public int NumberOfWomen { get; init;}
        public string? Source { get; init;}
        public DateOnly DateAdded { get; init;}
        public DateOnly ConfDate { get; init; }
        public float DiversityPercentage => (float)NumberOfWomen / TotalSpeakers;
        public int NumberOfMen => TotalSpeakers - NumberOfWomen;
    }
}