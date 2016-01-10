using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TextSummarizerV1
{
    class TextModel
    {
        
        //Upper list is the represents sentences while the inner represents the words
        private readonly List<List<string>> _text;

        public  TextModel()
        {
            _text = new List<List<string>>();
        }


        public TextModel(List<List<string>> text)
        {
            _text = text;
        }


        public  void AddSentence(List<string> wordList)
        {
            wordList.RemoveAll(word => word.Equals(""));

            _text.Add(wordList);
        }


        public List<List<string>> GetText()
        {
            return _text;
        }


        public int GetSentenceCount()
        {
            return _text.Count;
        }


        public int GetWordCountInSentence(int sentencePostion)
        {
            return _text[sentencePostion].Count;
        }


        public string GetWord(int sentencePostion, int wordPosition)
        {
            return _text[sentencePostion][wordPosition];
        }

        public List<string> GetSentence(int sentencePostion)
        {
            return _text[sentencePostion];
        }

        public string GetSentenceAsAString(int sentencePostion)
        {
            string sentence = string.Join(" ", _text[sentencePostion]);


            return sentence;
        }


        public List<string> GetSentencesAsStrings()
        {
            List<string> sentences = new List<string>();

            for (int i = 0; i < _text.Count; i++)
            {
                string combinedSentence = GetSentenceAsAString(i);

                if (combinedSentence.Equals("") == false)
                {
                    sentences.Add(combinedSentence);
                }

            }

            return sentences;
        }

        public List<string> GetAllWords()
        {
            List<string> allWords = new List<string>();

            foreach (var sentence in _text)
            {
               allWords.AddRange(sentence);
            }
            return allWords;
        }


        public void SetWord(int sentencePostion, int wordPosition, string inputWord)
        {
            _text[sentencePostion][wordPosition] = inputWord;
        }    
    }
}
