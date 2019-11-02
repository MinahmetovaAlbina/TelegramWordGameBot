using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramWordGamesBot
{
    abstract class Game
    {
        public string Name;
        public string Category;
        public abstract string Answer(int userId, string message);
        public abstract string FindWord(int userId, string pattern);
    }
}
