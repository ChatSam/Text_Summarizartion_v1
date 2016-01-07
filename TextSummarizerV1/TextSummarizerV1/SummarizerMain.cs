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

            TextModel text = new TextModel();

            //get the document
            string path = "..\\..\\Resources\\Test\\inputNews.txt";

            //pass it to a string 
            string initialText = File.ReadAllText(path);

            //send it to preprocessing 
            Preprocessor preprocessor = new Preprocessor(initialText);

            text = preprocessor.RunPreprocessor();

            TextModel unstemmedText = preprocessor.GetUnstemmedText();

            //send it to feature extraction
            FeatureExtractor featureExtractor = new FeatureExtractor(text, unstemmedText);

            // score weight for the cue-phrase feature
            double cuePhraseScoreWeighting = 0.3;

            Dictionary<int,double> sentenceScores = featureExtractor.RunFeatureExtractor(cuePhraseScoreWeighting);

            //send it to sentence selection and assembly
            SentenceSelector sentenceSelector = new SentenceSelector(text);

            //thresoldValue to controller the size of the summary
            double selectionThreshold = 0.7;

            List<int>rankedSentenceIds = sentenceSelector.RunSentenceSelector(sentenceScores, selectionThreshold);

            //send to generate summary
            SummaryGenerator summaryGenerator = new SummaryGenerator(text, unstemmedText);

            summaryGenerator.GenerateSummary(rankedSentenceIds);

            //evalute performance

            //save summary to doc

        }
    }
}
