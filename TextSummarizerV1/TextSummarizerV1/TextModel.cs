using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TextSummarizerV1
{
    class TextModel
    {
        
        //Upper list is the represents sentences while the inner represents the words
        private static List<List<string>> _text;

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


        public int SentenceCount()
        {
            return _text.Count;
        }


        public int WordCount(int sentencePostion)
        {
            return _text[sentencePostion].Count;
        }


        public string GetWord(int sentencePostion, int wordPosition)
        {
            return _text[sentencePostion][wordPosition];
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


        //do getters and setters
    }

    internal class words
    {
    }
}
