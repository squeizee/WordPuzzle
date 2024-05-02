using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;

namespace Utility
{
    public static class WordChecker
    {
        private static List<string> _words = new();

        private static Dictionary<string, int> wordScore = new Dictionary<string, int>()
        {
            { "E", 1 }, { "A", 1 }, { "O", 1 }, { "N", 1 }, { "R", 1 },
            { "T", 1 }, { "L", 1 }, { "S", 1 }, { "U", 1 }, { "I", 1 },
            { "D", 2 }, { "G", 2 },
            { "B", 3 }, { "C", 3 }, { "M", 3 }, { "P", 3 },
            { "F", 4 }, { "H", 4 }, { "V", 4 }, { "W", 4 }, { "Y", 4 },
            { "K", 5 },
            { "J", 8 }, { "X", 8 },
            { "Q", 10 }, { "Z", 10 }
        };

        public static bool IsValidWord(string word)
        {
            if (_words.Count == 0)
                _words = LevelSo.Instance.levelDictionary.text.Split('\n').ToList();

            return word.Length > 1 && _words.Contains(word.ToLower());
        }

        public static int GetTotalScore(List<string> words, int unusedLettersCount)
        {
            return GetWordScore(words) - (100 * unusedLettersCount);
        }

        public static int GetWordScore(List<string> words)
        {
            int totalScore = 0;

            foreach (var word in words)
            {
                int score = 0;
                foreach (char c in word)
                {
                    score += wordScore[c.ToString().ToUpper()];
                }

                totalScore += 10 * word.Length * score;
            }

            return totalScore;
        }
        public static int GetWordScore(string word)
        {
            int score = 0;
            foreach (char c in word)
            {
                score += wordScore[c.ToString().ToUpper()];
            }

            return 10 * word.Length * score;
        }

        public static bool HasValidWordWithCharacters(List<string> characters)
        {
            if (characters.Count < 2)
                return false;

            return _words.Any(word => word.All(c => characters.Contains(c.ToString().ToUpper())));
        }
    }
}