using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextSummarizerV1
{
    class Evaluator
    {
        private readonly TextModel _genratedSummary;
        private double Precision { get; set; }

        private double Recall { get; set; }

        private double FMeasure { get; set; }

        public Evaluator(TextModel generatedSummary)
        {
            _genratedSummary = generatedSummary;
        }

        /// <summary>
        /// Handles the evaluation process
        /// </summary>
        /// <param name="humanSummaryPath"></param>
        public void RunEvaluator(string humanSummaryPath)
        {
            // get the human generated summary 
            var humanSummary = GetHumanGeneratedSummary(humanSummaryPath);

            // calculate Recall
             Recall = CalculateRecall(humanSummary);

            // calculate Precsion
             Precision = CalculatePrecision(humanSummary);

            // calculate F- measure
             FMeasure = CalculateBalancedFmeasure(Precision, Recall);

            // prints the results
            PrintStats();
             
        }

        /// <summary>
        /// Parses the human generated summary to Text Model using the text file path
        /// </summary>
        /// <param name="humanSummaryPath">text file path for the human generated summary</param>
        /// <returns></returns>
        public TextModel GetHumanGeneratedSummary(string humanSummaryPath)
        {
            string summary = File.ReadAllText(humanSummaryPath);

            Preprocessor preprocessor = new Preprocessor(summary);

            preprocessor.RunPreprocessor();

            TextModel humanSummaryText = preprocessor.GetUnstemmedText();

            return humanSummaryText;
        }


        private double CalculateRecall(TextModel humanSummaryText)
        {
            // the count of sentences which exist in the generated summary
            int recallCount = GetBaseSentenceCount(humanSummaryText);

            int humanGenSentenceCount = humanSummaryText.GetSentenceCount();

            double recall = Math.Round((double)recallCount / humanGenSentenceCount,3) ;

            return recall;
        }

        private double CalculatePrecision(TextModel humanSummaryText)
        {
            int precisionCount = GetBaseSentenceCount(humanSummaryText);

            int genratedSentenceCount = _genratedSummary.GetSentenceCount();

            double precision = Math.Round((double)precisionCount / genratedSentenceCount, 3);

            return precision;
        }

        /// <summary>
        /// Gets the count of sentences which are both in the generated text and the human summarized text
        /// </summary>
        /// <param name="humanSummaryText"></param>
        /// <returns>count of sentences which exist in both texts</returns>
        private int GetBaseSentenceCount(TextModel humanSummaryText)
        {
            int baseSentenceCount = 0;

            var humanGeneratedSummarySentences = humanSummaryText.GetSentencesAsStrings();

            for (int i = 0; i < _genratedSummary.GetSentenceCount(); i++)
            {
                var generatedSentence = _genratedSummary.GetSentenceAsAString(i);

                if (humanGeneratedSummarySentences.Contains(generatedSentence))
                {
                    baseSentenceCount++;
                }
            }
            return baseSentenceCount;
        }


        private double CalculateBalancedFmeasure(double precision, double recall)
        {
            double fMeasureCalculation = (2 * precision * recall )/ (precision + recall);

            return Math.Round(fMeasureCalculation ,3) ;
        }


        public void PrintStats()
        {
            Console.WriteLine();
            Console.WriteLine("--------- Evaluation ---------");

            Console.WriteLine(
            "\n Precision : " + Precision + "\n" 
            + "\n Recal :" + Recall + "\n"
            + "\n F-Measure :" + FMeasure + "\n");

            Console.WriteLine("------------------------------");
        }

    }
}
