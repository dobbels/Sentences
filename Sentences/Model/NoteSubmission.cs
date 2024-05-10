using System.Globalization;
using Newtonsoft.Json;

namespace Sentences.Model
{
    public class NoteSubmission
    {
        // Is user by the cosmos client library to construct this type after retrieval
        public NoteSubmission()
        {
        }

        public NoteSubmission(string content)
        {
            Id =
            Content = content;
            MomentOfSubmission = DateTime.UtcNow;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime MomentOfSubmission { get; set; }
        public string MonthOfSubmission => $"{MomentOfSubmission.ToString("MMMM yyyy", DateTimeFormatInfo.InvariantInfo)}";
    }
}
