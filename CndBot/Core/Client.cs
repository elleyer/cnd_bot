using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace CndBot.Core
{
    public class Client
    {
        private const string API_TOKEN = "1760241379:AAGsnMnvQz3gPTJuFRdK69DbUEndodk6ceY";
        private static ITelegramBotClient _client;

        private const long MY_CHAT_ID = 424510699;

        private List<BotCommand> _botCommands = new List<BotCommand>();

        public void Init()
        {
            _client = new TelegramBotClient(API_TOKEN);
            
            _botCommands.AddRange(new []
            {
                new BotCommand
                {
                    Command = "start",
                    Description = "Розпочати роботу з ботом"
                },
                new BotCommand
                {
                    Command = "get_info",
                    Description = "Отримати необхідну інформацію"
                }
            });
            
            _client.SetMyCommandsAsync(_botCommands);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            _client.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            Console.ReadKey();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            if (update.Message is { } message)
            {
                if (message.Text != null && message.Text.StartsWith("/"))
                {
                    var command = CommandFactory.GetCommand(message.Text);
                    await Task.Run(() => command.ExecuteCommand(botClient, update), cancellationToken);
                }
                //Записатись у колектив
                //Перейти на сайт
                //Переглянути досупні фестивалі
                //Контакти
            }
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is ApiRequestException apiRequestException)
            {
                await botClient.SendTextMessageAsync(123, apiRequestException.ToString(),
                    cancellationToken: cancellationToken);
            }
        }
    }
}