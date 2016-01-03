using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextSummarizerV1
{
    class SentenceSelector
    {
        private static TextModel _text;

        public SentenceSelector(TextModel text)
        {
            _text = text;
        }

        /// <summary>
        /// Selects the sentences based on sentence scores using a threshold value
        /// </summary>
        /// <param name="sentenceScores">sentence scores</param>
        /// <param name="selectionThreshold"> threshold value</param>
        public List<int> RunSentenceSelector(Dictionary<int,double> sentenceScores, double selectionThreshold)
        {
            List<int> selectedSentenceNumbers = new List<int>();

            foreach (var sentenceNumber in sentenceScores.Keys)
            {
                if (sentenceScores[sentenceNumber] >= selectionThreshold)
                {
                    selectedSentenceNumbers.Add(sentenceNumber);
                }
            }

            return selectedSentenceNumbers;
        }
    }
}
