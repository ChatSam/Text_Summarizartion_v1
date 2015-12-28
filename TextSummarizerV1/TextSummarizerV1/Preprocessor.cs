using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextSummarizerV1
{
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

            IList<string> inTextNoStopWords = RemoveStopWords(ref inSenetences, stopWordFilePath);

        }


        private IList<string> SegmentSentences(string inTextLowerCase)
        {
            //todo: the function splits even sentences with commas - fix needed
            var inSentences = Regex.Split(inTextLowerCase, @"(?<=[\.!\?])\s+");

            return inSentences;
        }


        private IList<string> RemoveStopWords(ref IList<string> inSentences, string stopWordFilePath)
        {

            //initializing stop words
            var stopWordListRaw = File.ReadAllText(stopWordFilePath);

            var stopWordList = Regex.Split(stopWordListRaw, "\\W+");

            //remove stop words
            for (int i = 0; i < inSentences.Count; i++)
            {
                List<string> splitSentence = Regex.Split(inSentences[i], "\\W+").ToList();

                splitSentence.RemoveAll(word => stopWordList.Contains(word));

                inSentences[i] = string.Join("||", splitSentence);

            }

            return inSentences;
        }


       


        //remove suffixes [tokenization and word stemmer]

        //remove punctuation

        //return 

    }
}
