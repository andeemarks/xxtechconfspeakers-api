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
            return ConfDetails!.OrderBy(conf => conf.DateAdded).ToList().Last();
        }
    }

    public class SpeakerSummary : IEquatable<SpeakerSummary>
    {
        public string? Name { get; init; }
        public string? Location { get; init; }
        public string? Year { get; init; }
        public int TotalSpeakers { get; init;}
        public int NumberOfWomen { get; init;}
        public string? Source { get; init;}
        public DateOnly DateAdded { get; set;}
        public DateOnly ConfDate { get; init; }
        public float DiversityPercentage => (float)NumberOfWomen / TotalSpeakers;
        public int NumberOfMen => TotalSpeakers - NumberOfWomen;

        public bool Equals(SpeakerSummary? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Location == other.Location && Year == other.Year && TotalSpeakers == other.TotalSpeakers && NumberOfWomen == other.NumberOfWomen && Source == other.Source && DateAdded.Equals(other.DateAdded) && ConfDate.Equals(other.ConfDate);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((SpeakerSummary)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Location, Year, TotalSpeakers, NumberOfWomen, Source, DateAdded, ConfDate);
        }
    }
}
