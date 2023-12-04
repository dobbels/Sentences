using Newtonsoft.Json;

namespace SentenceCompletionApp.Model;

public class SentenceSubmission
{
    // Is used by the cosmos library to construct response of type ItemResponse<SentenceSubmission>
    public SentenceSubmission()
    {
    }

    public SentenceSubmission(SentenceSubmissionDto sentenceSubmissionDto)
    {
        if (sentenceSubmissionDto == null)
        {
            throw new ArgumentNullException(nameof(sentenceSubmissionDto));
        }

        Id = Guid.NewGuid().ToString();
        SentenceStem = sentenceSubmissionDto.SentenceStem;
        SentenceStemText = sentenceSubmissionDto.SentenceStem.Text;
        Ending = sentenceSubmissionDto.Ending;
        DateOfSubmission = DateTime.UtcNow;
    }

    [JsonProperty(PropertyName = "id")]
    public string Id { get; }

    public SentenceStem SentenceStem { get; }

    public string SentenceStemText { get; }

    public string Ending { get; }

    public DateTime DateOfSubmission { get; }
}
