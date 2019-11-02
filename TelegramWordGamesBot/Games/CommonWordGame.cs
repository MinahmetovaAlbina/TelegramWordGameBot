using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramWordGamesBot.CommonWordGame
{
    class CommonWordGame : Game.Game
    {
        // последняя буква из слова бота
        private Dictionary<int,char> LastUsedChar { get; set; }
        // слова которые уже были названы
        private Dictionary<int, List<string>> UsedWords;

        public CommonWordGame(string Name, string Category)
        {
            LastUsedChar = new Dictionary<int, char>();
            UsedWords = new Dictionary<int, List<string>>();
            this.Name = Name;
            this.Category = Category;
        }
        public override string Answer(int userId, string message)
        {

            if (!LastUsedChar.ContainsKey(userId))
            {
                LastUsedChar[userId] = ' ';
            }

            if (!UsedWords.ContainsKey(userId))
            {
                UsedWords[userId] = new List<string>();
            }

            if (!IsWordRight(userId,message) && UsedWords[userId].Count != 0)
            {
                return "Слово не подходит";
            }
            else if (IsWordUsed(userId,message))
            {
                return "Слово уже было использовано";
            }
            else
            {
                UsedWords[userId].Add(message);
                return Logic(userId, message);
            };
        }

        private string Logic(int userId, string text)
        {
            char userChar;
            if (text[text.Length - 1] == 'ь')
            {
                userChar = text.ToLower()[text.Length - 2];
            }
            else
            {
                userChar = text.ToLower()[text.Length - 1];
            }
            Console.WriteLine(userChar);
            var word = FindWord(userId,userChar.ToString());
            UsedWords[userId].Add(text.ToLower());
            if (word!="не нашлось")
            {
                if (word[word.Length - 1] == 'ь')
                {
                    LastUsedChar[userId] = word[word.Length - 2];
                }
                else
                {
                    LastUsedChar[userId] = word[word.Length - 1];
                }
            }
            Console.WriteLine(text);
            Console.WriteLine(LastUsedChar[userId]);
            UsedWords[userId].Add(word);
            return word;
        }

        private bool IsWordRight(int userId,string text)
        {
            var conn = DatavaseService.DatabaseConnector.GetConnection();
            string sql = "";
            sql = "select word.name from word, word_category,category where(word.name = '" + text + "' " +
                    "AND word.id = word_category.word_id " +
                    "and word_category.category_id = category.id " +
                    "and category.name = '" + Category + "')";
            NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
            conn.Open();
            var result = comm.ExecuteReader();
            var hasRows = result.HasRows;
            conn.Close();
            return LastUsedChar[userId] == text.ToLower()[0] && hasRows;

        }

        private bool IsWordUsed(int userId, string text)
        {
            return UsedWords[userId].Contains(text.ToLower());
        }

        public override string FindWord(int userId, string pattern)
        {
            var conn = DatavaseService.DatabaseConnector.GetConnection();
            string sql = "";
            sql = "select word.name from word, word_category,category where(word.name ilike '"+pattern+"%' " +
                    "AND word.id = word_category.word_id " +
                    "and word_category.category_id = category.id " +
                    "and category.name = '"+Category+"')";
            NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
            conn.Open();
            var result = comm.ExecuteReader();
            string word = "не нашлось";
            while (result.Read())
            {
                if (!UsedWords[userId].Contains(result["name"].ToString()))
                {
                    word = result["name"].ToString();
                    break;
                }
            }
            conn.Close();
            return word;
        }
    }
}
