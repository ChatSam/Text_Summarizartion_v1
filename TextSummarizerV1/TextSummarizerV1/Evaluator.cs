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
            //get the human generated summary 
            var humanSummary = GetHumanGeneratedSummary(humanSummaryPath);

            //calculate Recall
             Recall = CalculateRecall(humanSummary);

            // calculate Precsion
             Precision = CalculatePrecision(humanSummary);

            // calculate F- measure
             FMeasure = CalculateBalancedFmeasure(Precision, Recall);

            //prints the results
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


        private Double CalculateRecall(TextModel humanSummaryText)
        {
            // the count of sentences which exist in the generated summary
            int recallCount = 0;

            var generatedText = _genratedSummary.GetText();

            var humanGeneratedSummarySentences = humanSummaryText.GetText();

            int humanGenSentenceCount = humanGeneratedSummarySentences.Count();

            foreach (var sentence in humanGeneratedSummarySentences )
            {
                if (generatedText.Contains(sentence))
                {
                    recallCount++;
                }
            }

            double recall = Math.Round((double)recallCount / humanGenSentenceCount,3) ;

            return recall;
        }

        private Double CalculatePrecision(TextModel humanSummaryText)
        {
            int precisionCount = 0;

            int genratedSentenceCount = _genratedSummary.GetSentenceCount();

            var humanGeneratedSummary = humanSummaryText.GetText();

            foreach (var sentence in _genratedSummary.GetText())
            {
                if (humanGeneratedSummary.Contains(sentence))
                {
                    precisionCount++;
                }
            }

            double precision = Math.Round((double) precisionCount/genratedSentenceCount,3);

            return precision;
        }

        private Double CalculateBalancedFmeasure(double precision, double recall)
        {
            double fMeasureCalculation = (2 * precision * recall )/ (precision + recall);

            return fMeasureCalculation;
        }


        public void PrintStats()
        {
            Console.WriteLine("Precision : "+ Precision);
            Console.WriteLine("Recal :" + Recall);
            Console.WriteLine("F-Measure :" + FMeasure);
        }

    }
}
