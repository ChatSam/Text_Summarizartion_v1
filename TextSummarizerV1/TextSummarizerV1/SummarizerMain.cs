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
            double selectionThreshold = 0.55;

            // ----------------------------------

            //get the document
            string path = "..\\..\\Resources\\Test\\inputNews.txt";
           
            //pass the document to a string 
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

            //save summary to doc

        }
    }
}
