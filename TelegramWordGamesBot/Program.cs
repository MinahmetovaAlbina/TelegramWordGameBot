
using System.Collections.Generic;

namespace TelegramWordGamesBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var games = new List<Game>
            {
                new CommonWordGame("Игра в слова(Города)", "города мира"),
                new CommonWordGame("Игра в слова(Машины)", "марки автомобилей")
            };


            CommunicationService.Start(games);
        }
    }
}
