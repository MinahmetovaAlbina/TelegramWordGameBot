
using System.Collections.Generic;

namespace TelegramWordGamesBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var games = new List<Game.Game>
            {
                new CommonWordGame.CommonWordGame("Игра в слова(Города)", "города мира"),
                new CommonWordGame.CommonWordGame("Игра в слова(Машины)", "марки автомобилей")
            };


            TelegramCommunicationService.CommunicationService.Start(games);
        }
    }
}
