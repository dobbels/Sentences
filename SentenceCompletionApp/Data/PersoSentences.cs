using SentenceCompletionApp.Model;

namespace SentenceCompletionApp.Data
{
    public class PersoSentences
    {
        public static SentenceStem[] GetSentences()
        {
            return new SentenceStem[]
                {
                    new SentenceStem("Wat ik voelde thuis wanneer we geterroriseerd werden was..."),
                    new SentenceStem("Wat er door me heen ging thuis wanneer we geterroriseerd werden was..."),
                };
        }

        public static SentenceStem GetRandomSentence()
        {
            var sentences = GetSentences();

            var random = new Random();
            var sentenceNumber = random.Next(1, sentences.Count() + 1);
            var randomSentence = sentences[sentenceNumber];

            return randomSentence;
        }
    }
}
