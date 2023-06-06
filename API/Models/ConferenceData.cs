using System.Collections;

namespace API.Models
{
    public class ConferenceData
    {
        public ConferenceData()
        {

        }

        public static List<SpeakerSummary> SpeakerSummary()
        {
            return new List<SpeakerSummary>() { new SpeakerSummary("Hello", "Here", "reddit") };
        }
    }

    public class SpeakerSummary
    {
        public SpeakerSummary(string name, string location, string source)
        {
            Name = name;
            Location = location;
            Source = source;
        }

        public string Name { init; get; }
        public string Location { get; init; }
        public int Year { get; init;  }
        public int TotalSpeakers { get; init; }
        public int NumberOfWomen { get; init; }
        public string Source { init; get; }
        public DateOnly DateAdded { get; init; }
        public DateOnly ConfDate { get; init; }
    }
}