using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TextSummarizerV1
{
    class SummaryGenerator
    {

        private readonly TextModel _text;

        public SummaryGenerator(TextModel text)
        {
            _text = text;
        }

        public void GenerateSummary(List<int> sentenceScores)
        {
            TextModel summary = AssembleSummarySentences(sentenceScores);

            PrintSummary(summary);
        }

        public void PrintSummary(TextModel summary)
        {
            for (int i = 0; i < summary.SentenceCount(); i++)
            {
                String outputSentence = " - " + string.Join(" " ,summary.GetSentence(i));

                Console.WriteLine(outputSentence);
            }

            Console.ReadKey();
        }


        /// <summary>
        /// Maps the sentences from text to selected sentence Ids to create the summary
        /// </summary>
        /// <param name="sentenceIds"></param>
        /// <returns></returns>
        public  TextModel AssembleSummarySentences(List<int> sentenceIds)
        {
            TextModel summary = new TextModel();

            foreach (var sentenceId in sentenceIds)
            {
                
                var sentence = _text.GetSentence(sentenceId);

                summary.AddSentence(sentence);
            }

             return summary;
        }



    }
}