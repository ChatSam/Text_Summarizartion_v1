﻿using System;
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

        private TextModel summary { get; set; }

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

            summary = AssembleSummarySentences(sentenceIds, _unstemmedText);

            PrintSummary(summary);
        }

        public void PrintSummary(TextModel summary)
        {
            Console.WriteLine('\n');

            for (int i = 0; i < summary.GetSentenceCount(); i++)
            {
                String outputSentence = " - " + summary.GetSentenceAsAString(i);

                Console.WriteLine(outputSentence);
            }
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

        public TextModel GetGeneratedSummary()
        {
            return summary;
        }


    }
}