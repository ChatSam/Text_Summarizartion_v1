using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TextSummarizerV1
{
    class FeatureExtractor
    {
        private static TextModel _text;

        public FeatureExtractor(TextModel text)
        {
            _text = text;
        }

        public Dictionary<int, double> RunFeatureExtractor()
        {
            Dictionary<string, int> termFrequencyValues = CalculateTermFrequency();

            Dictionary<string, int> sentenceFrequencyValues = CalculateSentenceFrequencyPerWord(termFrequencyValues);

            Dictionary<string, double> inverseSentenceFrequencyValues =
                CalculateInverseSentenceFrequency(sentenceFrequencyValues, termFrequencyValues);

            Dictionary<string, double> termFreqInverseSentenceFreqValues =
                CalculateTermFreqInverseTermFreq(termFrequencyValues, inverseSentenceFrequencyValues);

            Dictionary<string, double> normalizedTermFreqInverseSentFreqValues =
                Normalize(termFreqInverseSentenceFreqValues);

            Dictionary<int, double> sentenceScore = CalculateSentenceScores(normalizedTermFreqInverseSentFreqValues);

            return sentenceScore;
        }

        private static Dictionary<string, int> CalculateTermFrequency()
        {
            Dictionary<string, int> termFrequency = new Dictionary<string, int>();

            List<string> wordList = _text.GetAllWords();

            foreach (var word in wordList)
            {
                if (termFrequency.ContainsKey(word))
                {
                    termFrequency[word] += 1;
                }
                else
                {
                    termFrequency.Add(word, 1);
                }
            }

            return termFrequency;
        }

        private static Dictionary<string, int> CalculateSentenceFrequencyPerWord(Dictionary<string,int> wordDictionary)
        {
            // this dictionary has the no. of the sentences in which the term i occurs
            Dictionary<string, int> wordSenetenceCounter = new Dictionary<string, int>();

            foreach (var sentence in _text.GetText())
            {
                var distinctWords = sentence.Distinct();

                foreach (var word in distinctWords)
                {
                    if (wordSenetenceCounter.ContainsKey(word))
                    {
                        wordSenetenceCounter[word] += 1;
                    }
                    else
                    {
                        wordSenetenceCounter[word] = 1;
                    }
                }
            }

            return wordSenetenceCounter;
        }


        private static Dictionary<string, double> CalculateInverseSentenceFrequency(
            Dictionary<string, int> sentenceFrequency, Dictionary<string, int> termFrequencyValues)
        {
            Dictionary<string,double> inverseSentenceFrequencyValues = new Dictionary<string, double>();

            foreach (var word in termFrequencyValues.Keys)
            {
                double ni = sentenceFrequency[word];

                double N = _text.SentenceCount();

                double value = N/ni;

                double inverseSentenceFrequency = Math.Log(value);

                int roundOffValue = 4;

                inverseSentenceFrequencyValues[word] =  Math.Round(inverseSentenceFrequency, roundOffValue);
            }

            return inverseSentenceFrequencyValues;
        }

        private static Dictionary<string, double> CalculateTermFreqInverseTermFreq(
            Dictionary<string, int> termFrequency, Dictionary<string, double> inverseTermFrequency)
        {
            Dictionary<string, double> termFreqInverseSentenceFreqValues = new Dictionary<string, double>();

            foreach (var word in termFrequency.Keys)
            {
                double calculation = termFrequency[word]*inverseTermFrequency[word];

                termFreqInverseSentenceFreqValues[word] = calculation;
            }

            return termFreqInverseSentenceFreqValues;
        }

        private static Dictionary<string, double> Normalize(Dictionary<string, double> termFreqInverseSentenceFreqValues)
        {
            Dictionary<string,double> normalizedValues  = new Dictionary<string, double>();

            double totalTermValues = termFreqInverseSentenceFreqValues.Values.Sum();

            double maxNormalizedValue = termFreqInverseSentenceFreqValues.Values.Max() / totalTermValues;

            foreach (var word in termFreqInverseSentenceFreqValues.Keys)
            {
                double normalized = (termFreqInverseSentenceFreqValues[word])/ totalTermValues;

                normalizedValues[word] = FineNormalize(normalized, maxNormalizedValue);
            }

            return normalizedValues;
        }

        private static double FineNormalize(double normalizedValue, double maxNormalizedValue)
        {
            double fineNormalized = normalizedValue / maxNormalizedValue;

            return fineNormalized;
        }

        private static Dictionary<int, double> CalculateSentenceScores(Dictionary<string, double> normalizedTermFreqInverseSentFreqValues)
        {
            Dictionary<int,double> rankedSentence = new Dictionary<int, double>();

            for (int i = 0; i < _text.SentenceCount(); i++)
            {
                double sentenceScore = 0;

                for (int k = 0; k < _text.WordCount(i); k++)
                {
                    sentenceScore += normalizedTermFreqInverseSentFreqValues[_text.GetWord(i, k)];
                }

                rankedSentence[i] = sentenceScore;
            }
            return rankedSentence;
        }

      
    }
}

