using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextSummarizerV1
{
    ///
    /// Text Summarizer Version 1
    /// Author - Chatura Samarasinghe
    /// References - Porter's alogoritm - http://tartarus.org/~martin/PorterStemmer/csharp3.txt
    ///              - Author - Brad Patton - http://ratborg.blogspot.com/




    class Preprocessor
    {
        //todo: check if the static keyword is needed
        private static string _initialText;


        public Preprocessor(string initialText)
        {
            _initialText = initialText;
        }


        /// <summary>
        /// Manages the tasks of the preprocessor
        /// </summary>
        public void RunPreprocessor()
        {
            TextModel text = new TextModel();

            //convert to lower case (string text) 
            string inTextLowerCase = _initialText.ToLower();

            string stopWordFilePath = "../../Resources/Preprocessing/standard-stopwords.txt";

            IList<string> inSenetences = SegmentSentences(inTextLowerCase);

            TextModel unStemmedText = GetRawText(inSenetences);

            TextModel inTextNoStopWords = RemoveStopWordsAndPunctuation(ref inSenetences, stopWordFilePath);

            //IList<string> inTextWordStemmed = WordStemmer(inTextNoStopWords);

            TextModel inTextWordStemmed = WordStemmer(inTextNoStopWords);

            

        }


        private IList<string> SegmentSentences(string inTextLowerCase)
        {
            //todo: the function splits even sentences with commas, or numbers with decimal points - fix needed
            var inSentences = Regex.Split(inTextLowerCase, @"(?<=[\.!\?])\s+");

            return inSentences;
        }

        /// <summary>
        /// Removees stop words and punctuation
        /// </summary>
        /// <param name="inSentences"></param>
        /// <param name="stopWordFilePath"></param>
        /// <returns></returns>
        private TextModel RemoveStopWordsAndPunctuation(ref IList<string> inSentences, string stopWordFilePath)
        {

            TextModel text = new TextModel();

            //initializing stop words
            var stopWordListRaw = File.ReadAllText(stopWordFilePath);

            var stopWordList = Regex.Split(stopWordListRaw, "\\W+");

            //remove stop words
            for (int i = 0; i < inSentences.Count; i++)
            {
                var wordsFromSentence = TokenizeSentence(inSentences[i]);

                wordsFromSentence.RemoveAll(word => stopWordList.Contains(word));

                inSentences[i] = string.Join("|", wordsFromSentence);

                // add the cleaned sentences to the TextModel
                text.AddSentence(wordsFromSentence);

            }

            return text;
        }


        private static TextModel GetRawText(IList<string> inSentences)
        {
            TextModel rawText = new TextModel();

            for (int i = 0; i < inSentences.Count; i++)
            {
                var wordsFromSentence = TokenizeSentence(inSentences[i]);

                // add the un-stemmed text
                rawText.AddSentence(wordsFromSentence);
            }

            return rawText;
        }

        //todo: no proper tokenizer currently implemented
        private static List<string> TokenizeSentence(string inSentence)
        {
            List<string> tokenizedSentence = Regex.Split(inSentence, "\\W+", RegexOptions.IgnorePatternWhitespace).ToList();

            tokenizedSentence.RemoveAll(word => word.Equals(""));

            return tokenizedSentence;
        }


        public static TextModel WordStemmer(TextModel text)
        {
            Stemmer stemmer = new Stemmer();

            for (int i = 0; i < text.SentenceCount(); i++)
            {
                for (int k = 0; k < text.WordCount(i); k++)
                {
                    var stemmedWord = stemmer.StemWord(text.GetWord(i,k));

                    text.SetWord(i,k, stemmedWord);
                }
            }
            return text;
        }
    }
}
