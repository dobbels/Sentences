namespace SentenceCompletionApp.Data
{
    public class SentenceStemService
    {
        private string[] sentenceStems = new string[] {
            "Today is a great day because...",
            "The best way to start the day is...",
            "I feel happiest when..."
        };

        public Task<SentenceStem> GetNextSentenceStemAsync()
        {
            var random = new Random();
            var currentStem = sentenceStems[random.Next(sentenceStems.Length)];
            return Task.FromResult(new SentenceStem(currentStem));
        }

        public Task PersistUserInputAsync(SentenceSubmission input)
        {
            Console.WriteLine(input.SentenceStem.Text + " - " + input.Ending);
            return Task.FromResult("");
        }
    }
}
