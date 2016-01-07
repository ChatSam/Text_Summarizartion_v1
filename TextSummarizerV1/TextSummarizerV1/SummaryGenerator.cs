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
        private readonly TextModel _unstemmedText;

        public SummaryGenerator(TextModel text, TextModel orignalUnstemmedText)
        {
            _text = text;
            _unstemmedText = orignalUnstemmedText;
        }

        /// <summary>
        /// controls all the sumary generation task
        /// </summary>
        /// <param name="sentenceIds">ids of selected sentences</param>
        public void GenerateSummary(List<int> sentenceIds)
        {
            TextModel rawSummary = AssembleSummarySentences(sentenceIds, _text);

            TextModel summary = AssembleSummarySentences(sentenceIds, _unstemmedText);

            PrintSummary(summary);
        }

        public void PrintSummary(TextModel summary)
        {
            for (int i = 0; i < summary.GetSentenceCount(); i++)
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
        /// <param name="text"></param>
        /// <returns></returns>
        public  TextModel AssembleSummarySentences(List<int> sentenceIds,TextModel text)
        {
            TextModel summary = new TextModel();

            foreach (var sentenceId in sentenceIds)
            {
                var sentence = text.GetSentence(sentenceId);

                summary.AddSentence(sentence);
            }
             return summary;
        }

    
    }
}