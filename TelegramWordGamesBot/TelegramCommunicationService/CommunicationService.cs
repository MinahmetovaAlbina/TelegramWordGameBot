using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace TelegramWordGamesBot
{
    class CommunicationService
    {
        public static TelegramBotClient Bot;
        public static Dictionary<int, Game> Users;
        public static List<Game> Games;
        public static void Start(List<Game> ListOfGames)
        {
            Games = ListOfGames;
            Users = new Dictionary<int, Game>();
            var wp = new HttpToSocks5Proxy("150.109.195.10", 1080);
            wp.ResolveHostnamesLocally = true;
            Bot = new TelegramBotClient("904574352:AAF6VYhJWZ7kUCKgx6l5rVQgOnfcmv45uLg", wp);
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnCallbackQuery += Bot_OnCallbackQuery;
            Bot.StartReceiving();
            Console.WriteLine("Bot started");
            Console.ReadLine();
        }

        private static async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            var message = e.CallbackQuery;
            Console.WriteLine(message.Data);
            foreach (var game in Games)
            {
                if (game.Name == e.CallbackQuery.Data)
                {
                    Users[message.From.Id] = game;
                    await Bot.SendTextMessageAsync(message.From.Id, game.Name+" началась! Введите первое слово");
                    Console.WriteLine("Игра началась");
                    break;
                }
            }
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != MessageType.Text)
                return;
            if (Users.ContainsKey(message.From.Id))
            {
                await Bot.SendTextMessageAsync(message.From.Id, Users[message.From.Id].Answer(message.From.Id, message.Text));
            }
            else
            {
                await MakeCommands(message);
            }
        }

        private static async Task MakeCommands(Message message)
        {
            switch (message.Text)
            {
                case "/start":
                    await Bot.SendTextMessageAsync(message.From.Id, "Привет!\n Перейти к списку игр /List_of_games");
                    break;
                case "/List_of_games":
                    var buttons = new InlineKeyboardButton[Games.Count];
                    for (int i = 0; i < Games.Count; i++)
                    {
                        buttons[i] = InlineKeyboardButton.WithCallbackData(Games[i].Name);
                    }
                    var inlinekeyboard = new InlineKeyboardMarkup(new[]
                    {
                        buttons
                    }
                    );
                    await Bot.SendTextMessageAsync(message.From.Id, "Выберите игру", replyMarkup: inlinekeyboard);
                    break;
            }
        }
}
}
