namespace Sentences.Model;

public class SentenceSubmissionDto
{
    public SentenceSubmissionDto(SentenceStem? sentenceStem, string ending)
    {
        if (sentenceStem == null)
        {
            throw new ArgumentNullException(nameof(sentenceStem));
        }

        SentenceStem = sentenceStem;
        Ending = ending;
    }

    public SentenceStem SentenceStem { get; set; }
    public string Ending { get; set; }
}
