using System.Collections;

namespace API.Models
{
    public class ConferenceData
    {
        public ConferenceData()
        {

        }

        public static IEnumerable SpeakerSummary()
        {
            return new List<string>() { "Hello, World!" };
        }
    }
}