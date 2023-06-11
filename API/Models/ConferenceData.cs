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
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };
            ConfDetails = JsonSerializer.Deserialize<List<SpeakerSummary>>(confData, options);
        }

        public ConferenceData(string dataSourcePath) : this(new FileStream(dataSourcePath + "/confs.json", new FileStreamOptions())) { }

        private List<SpeakerSummary>? ConfDetails { get; }

        public List<SpeakerSummary> SpeakerSummary()
        {
            return ConfDetails!;
        }

        public SpeakerSummary MostRecentlyAdded()
        {
            return ConfDetails![0];
        }
    }

    public class SpeakerSummary
    {
        public string? Name { get; set; }
        public string? Location { get; init; }
        public string? Year { get; init; }
        public int TotalSpeakers { get; set;}
        public int NumberOfWomen { get; set;}
        public string? Source { get; init;}
        public DateOnly DateAdded { get; set;}
        public DateOnly ConfDate { get; init; }
        public float DiversityPercentage => (float)NumberOfWomen / TotalSpeakers;
        public int NumberOfMen => TotalSpeakers - NumberOfWomen;
    }
}
