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
        private readonly TextModel _text;
        private readonly TextModel _unstemmedText;

        public FeatureExtractor(TextModel text, TextModel unstemmedText)
        {
            _text = text;
            _unstemmedText = unstemmedText;
        }

        /// <summary>
        ///  Handles all the feature extraction tasks
        /// </summary>
        /// <param name="cuePhraseScoreWeighting"> weighting for the cue phrase score</param>
        /// <returns></returns>
        public Dictionary<int, double> RunFeatureExtractor(double cuePhraseScoreWeighting)
        {
            //TF - ISF feature
            Dictionary<int, double> sentenceScoresTermFreqInvSentFreq =  RunTermFreqInverseSentFreqFeature();
          
            List<string> cuePhraseList= new List<string>()
            {
                "the best"," the most important","this paper","this article","the document",
                "we concluded","in conclusion"
            };

            // Cue Phrase feature
            Dictionary<int, double> sentenceScoresforCuePhrase = RunCuePhraseFeature(cuePhraseList, cuePhraseScoreWeighting);

            //Added score of all the features
            Dictionary<int, double> finalSentenceScores = AddSentenceScores(sentenceScoresTermFreqInvSentFreq,
                sentenceScoresforCuePhrase);

            return finalSentenceScores;
        }


        /// <summary>
        /// Combines all the sentence scores from each feature in to one score set
        /// </summary>
        /// <param name="sentenceScoresTermFreqInvSentFreq"></param>
        /// <param name="sentenceScoresforCuePhrase"></param>
        /// <returns></returns>
        private Dictionary<int, double> AddSentenceScores(Dictionary<int, double> sentenceScoresTermFreqInvSentFreq,
            Dictionary<int, double> sentenceScoresforCuePhrase)
        {
            Dictionary<int,double> finalSentenceScores =  new Dictionary<int, double>();

            for (int sentence = 0; sentence < _text.GetSentenceCount(); sentence++)
            {
                finalSentenceScores[sentence] = sentenceScoresTermFreqInvSentFreq[sentence] +
                                                sentenceScoresforCuePhrase[sentence];
            }
            return finalSentenceScores;
        }


        /// <summary>
        /// Handles the Cue Phrase Feature
        /// </summary>
        /// <param name="cuePhraseList"></param>
        /// <param name="scoreWeighting"></param>
        /// <returns></returns>
        private Dictionary<int, double> RunCuePhraseFeature(List<string>cuePhraseList, double scoreWeighting)
        {
            Dictionary<int, double> sentenceScore = new Dictionary<int, double>();

            for (int sentenceNumber = 0; sentenceNumber< _unstemmedText.GetSentenceCount() ; sentenceNumber++)
            {
                var sentence = _unstemmedText.GetSentence(sentenceNumber);

                string formedSentence = string.Join(" ", sentence );

                var cuePhrasesInSentence = cuePhraseList.Any(phrase => formedSentence.Contains(phrase));

                //if there is a cue phrase exists, add the weighting score to the sentence (per each word)
                if (cuePhrasesInSentence)
                {
                    //todo: tweakable point to improve algortihm

                    // the score is added to each word in the sentence per every cue phrase in the sentence.
                    double scoreToAdd = scoreWeighting * _unstemmedText.GetWordCountInSentence(sentenceNumber) ;

                    sentenceScore[sentenceNumber] = scoreToAdd;
                }
                else
                {
                    sentenceScore[sentenceNumber] = 0;
                }
            }

            return sentenceScore;
        }


        /// <summary>
        /// Manages the Term Frequency - Inverse Sentence Frequency Feature functions
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, double> RunTermFreqInverseSentFreqFeature()
        {
            Dictionary<string, int> termFrequencyValues = CalculateTermFrequency();

            Dictionary<string, int> sentenceFrequencyValues = CalculateSentenceFrequencyPerWord();

            Dictionary<string, double> inverseSentenceFrequencyValues =
                CalculateInverseSentenceFrequency(sentenceFrequencyValues, termFrequencyValues);

            Dictionary<string, double> termFreqInverseSentenceFreqValues =
                CalculateTermFreqInverseTermFreq(termFrequencyValues, inverseSentenceFrequencyValues);

            Dictionary<string, double> normalizedTermFreqInverseSentFreqValues =
                NormalizeIsfValues(termFreqInverseSentenceFreqValues);

            Dictionary<int, double> sentenceScore = CalculateSentenceScores(normalizedTermFreqInverseSentFreqValues);

            return sentenceScore;
        }

        /// <summary>
        /// Calculate the number of times a term is repeated
        /// </summary>
        /// <returns></returns>
        private  Dictionary<string, int> CalculateTermFrequency()
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

        private  Dictionary<string, int> CalculateSentenceFrequencyPerWord()
        {
            //this dictionary has the no. of the sentences in which a term (variable- word) occurs
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


        private  Dictionary<string, double> CalculateInverseSentenceFrequency(
            Dictionary<string, int> sentenceFrequency, Dictionary<string, int> termFrequencyValues)
        {
            Dictionary<string,double> inverseSentenceFrequencyValues = new Dictionary<string, double>();

            foreach (var word in termFrequencyValues.Keys)
            {
                double ni = sentenceFrequency[word];

                double N = _text.GetSentenceCount();

                double value = N/ni;

                double inverseSentenceFrequency = Math.Log(value);

                int roundOffValue = 4;

                inverseSentenceFrequencyValues[word] =  Math.Round(inverseSentenceFrequency, roundOffValue);
            }

            return inverseSentenceFrequencyValues;
        }



        /// <summary>
        /// Primary Calculator function for TF-ISF feature
        /// </summary>
        /// <param name="termFrequency"></param>
        /// <param name="inverseTermFrequency"></param>
        /// <returns></returns>
        private  Dictionary<string, double> CalculateTermFreqInverseTermFreq(
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

        /// <summary>
        /// Normalizes Inverse Frequency values
        /// </summary>
        /// <param name="termFreqInverseSentenceFreqValues"></param>
        /// <returns></returns>
        private  Dictionary<string, double> NormalizeIsfValues(Dictionary<string, double> termFreqInverseSentenceFreqValues)
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


        /// <summary>
        /// Improves on the normalization values to get values onto a 1-0 scale
        /// </summary>
        /// <param name="normalizedValue"></param>
        /// <param name="maxNormalizedValue"></param>
        /// <returns></returns>
        private  double FineNormalize(double normalizedValue, double maxNormalizedValue)
        {
            double fineNormalized = normalizedValue / maxNormalizedValue;

            return fineNormalized;
        }


        private  Dictionary<int, double> CalculateSentenceScores(Dictionary<string, double> normalizedTermFreqInverseSentFreqValues)
        {
            Dictionary<int,double> rankedSentence = new Dictionary<int, double>();

            for (int i = 0; i < _text.GetSentenceCount(); i++)
            {
                double sentenceScore = 0;

                for (int k = 0; k < _text.GetWordCountInSentence(i); k++)
                {
                    sentenceScore += normalizedTermFreqInverseSentFreqValues[_text.GetWord(i, k)];
                }

                rankedSentence[i] = sentenceScore;
            }

            //normalize the sentence scores
            var highestSentenceScore = rankedSentence.Values.Max();

            for (int i = 0; i < _text.GetSentenceCount(); i++)
            {
                rankedSentence[i] = rankedSentence[i]/highestSentenceScore;
            }


            return rankedSentence;
        }

      
    }
}

