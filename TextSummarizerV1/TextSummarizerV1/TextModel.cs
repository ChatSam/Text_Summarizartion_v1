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
        
        public static List<string> words;

    
    

        //Upper list is the represents sentences while the inner represents the words
        private static List<List<string>> Text { get; set; }


        public  TextModel()
        {
        
            Text = new List<List<string>>();

        }

        public  void AddSentence(List<string> wordList)
        {
            wordList.RemoveAll(word => word.Equals(""));

            Text.Add(wordList);

        }

        public static List<List<string>> GetText()
        {
            return Text;
        }

        public int SentenceCount()
        {
            return Text.Count;
        }

        public int WordCount(int sentencePostion)
        {
            return Text[sentencePostion].Count;
        }

        public string GetWord(int sentencePostion, int wordPosition)
        {
            return Text[sentencePostion][wordPosition];
        }

        public void SetWord(int sentencePostion, int wordPosition, string inputWord)
        {
            Text[sentencePostion][wordPosition] = inputWord;
        }


        //do getters and setters
    }

    internal class words
    {
    }
}
