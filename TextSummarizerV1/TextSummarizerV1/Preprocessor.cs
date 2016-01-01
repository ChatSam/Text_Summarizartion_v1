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
            //convert to lower case (string text) 
            string inTextLowerCase = _initialText.ToLower();

            string stopWordFilePath = "../../Resources/Preprocessing/standard-stopwords.txt";

            IList<string> inSenetences = SegmentSentences(inTextLowerCase);

            IList<string> inTextNoStopWords = RemoveStopWordsAndPunctuation(ref inSenetences, stopWordFilePath);

            IList<string> inTextWordStemmed = WordStemmer(inTextNoStopWords);

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
        private IList<string> RemoveStopWordsAndPunctuation(ref IList<string> inSentences, string stopWordFilePath)
        {

            //initializing stop words
            var stopWordListRaw = File.ReadAllText(stopWordFilePath);

            var stopWordList = Regex.Split(stopWordListRaw, "\\W+");

            //remove stop words
            for (int i = 0; i < inSentences.Count; i++)
            {
                List<string> splitSentence = Regex.Split(inSentences[i], "\\W+").ToList();

                splitSentence.RemoveAll(word => stopWordList.Contains(word));

                inSentences[i] = string.Join("|", splitSentence);

            }

            return inSentences;
        }


        public static IList<string> WordStemmer(IList<string> inSentences)
        {
            Stemmer stemmer = new Stemmer();

            for (int i = 0; i < inSentences.Count; i++)
            {
                var words = inSentences[i].Split('|');

                for (int k = 0; k < words.Length; k++)
                {
                    words[k] = stemmer.StemWord(words[k]);
                }

                inSentences[i] = string.Join("|",words);
            }

            return inSentences;
        }


     
        //remove punctuation

        //return 

    }
}
