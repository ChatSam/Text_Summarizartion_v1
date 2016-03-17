using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextSummarizerV1
{
    class SummarizerMain
    {
        static void Main(string[] args)
        {

            //------ Adjustable parameters ------ 

            // score weight for the cue-phrase feature
            double cuePhraseScoreWeighting = 0.3;

            //thresoldValue to controller the size of the summary
            double defaultSelectionThreshold = 0.5;

            // ----------------------------------

            double selectionThreshold;

            //Innitializes the program and prompts user for the file path of the document
            var path = InnitializeProgram(out selectionThreshold, defaultSelectionThreshold);
            
            //pass it to a string 
            string initialText = File.ReadAllText(path);

            //send it to preprocessing 
            Preprocessor preprocessor = new Preprocessor(initialText);

            var text = preprocessor.RunPreprocessor();

            TextModel unstemmedText = preprocessor.GetUnstemmedText();

            //send it to feature extraction
            FeatureExtractor featureExtractor = new FeatureExtractor(text, unstemmedText);

            Dictionary<int,double> sentenceScores = featureExtractor.RunFeatureExtractor(cuePhraseScoreWeighting);

            //send it to sentence selection and assembly
            SentenceSelector sentenceSelector = new SentenceSelector(text);

            List<int>rankedSentenceIds = sentenceSelector.RunSentenceSelector(sentenceScores, selectionThreshold);

            //send to generate summary
            SummaryGenerator summaryGenerator = new SummaryGenerator(text, unstemmedText);

            summaryGenerator.GenerateSummary(rankedSentenceIds);

            //evalaute performance
            Evaluator summaryEvaluator = new Evaluator(summaryGenerator.GetGeneratedSummary());

            string humanSummaryPath = "..\\..\\Resources\\Test\\humanSummaryNews.txt";

            summaryEvaluator.RunEvaluator(humanSummaryPath);

            Console.ReadKey();


        }

        /// <summary>
        ///  Innitializes the program an prompts the user for the file path of the 
        /// document to be summarized
        /// </summary>
        /// <param name="selectionThreshold"> determines the size of the summary to be generated.</param>
        /// <param name="defaultSelectionThreshold">the default threshold value</param>
        /// <returns> the path of the document</returns>
        private static string InnitializeProgram( out double selectionThreshold, double defaultSelectionThreshold)
        {

            string heading = "\t ----------- Text Summarizer C v1.0 ----------- \t \n";

            Console.Write(new string(' ', (Console.WindowWidth - heading.Length)/2));

            Console.WriteLine(heading);

            Console.Write("Enter a document path to summarize : ");

            string path = Console.ReadLine();

            while (File.Exists(path) == false)
            {
                Console.WriteLine("Invalid path");

                Console.Write("Enter the file path of a document to summarize : ");

                path = Console.ReadLine();
            }

            Console.Write("Enter a threshold value ( between 0 and 1) : ");

            string threshold = Console.ReadLine();

            if (double.TryParse(threshold, out selectionThreshold) == false )
            {
                selectionThreshold = defaultSelectionThreshold;

                Console.WriteLine("using default threshold of value " + selectionThreshold);
            }
           
            return path;
        }
    }
}
