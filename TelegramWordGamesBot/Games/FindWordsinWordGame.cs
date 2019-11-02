using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace TelegramWordGamesBot.Games
{
    class FindWordsinWordGame : Game
    {
        public FindWordsinWordGame(string Name, string Category)
        {
            this.Name = Name;
            this.Category = Category;
        }

        public override string Answer(int userId, string message)
        {
            throw new NotImplementedException();
        }

        protected override string FindWord(int userId, string pattern)
        {
            throw new NotImplementedException();
        }
    }
}
