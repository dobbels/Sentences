namespace SentenceCompletionApp.Data
{
    public class SentenceSubmission
    {
        public SentenceStem SentenceStem { get; set; }

        public SentenceSubmission(SentenceStem sentenceStem, string ending)
        {
            SentenceStem = sentenceStem;
            Ending = ending;
        }

        public string Ending { get; set; }
    }
}
