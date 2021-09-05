using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CndBot.Core.Actions;
using CndBot.Core.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CndBot.Core
{
    public class Client
    {
        public static DataBaseProvider DataBaseProvider;
        
        public const string SITE_URL = "http://tvorchist.in.ua";
        
        private const string API_TOKEN = "1760241379:AAGsnMnvQz3gPTJuFRdK69DbUEndodk6ceY";
        
        private static ITelegramBotClient _client;

        private const long MAIN_CHAT_ID = 424510699;
        private const long SIGN_CHAT_ID = -1;

        private List<BotCommand> _botCommands = new List<BotCommand>();
        
        private Dictionary<RegisterFormAction, FormDataModel> _stagesById
            = new Dictionary<RegisterFormAction, FormDataModel>();

        public void Init()
        {
            DataBaseProvider = new DataBaseProvider(new DbContextOptions<DataBaseProvider>());

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
            if (update.Message is {From: { }} message)
            {
                /*botClient.SendTextMessageAsync(update.Message.Chat, "123");
                return;*/
                var userId = message.From.Id;
                
                //Command handler
                if (message.Text != null && message.Text.StartsWith("/"))
                {
                    var command = CommandFactory.GetCommand(message.Text);
                    
                    try
                    {
                        await Task.Run(() => command.ExecuteCommand(botClient, update), cancellationToken);
                    }
                    catch (ApiRequestException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    
                    return;
                }

                //Message logic handler
                if(message.Text != null && !message.Text.StartsWith("/"))
                {
                    //todo: Refactor
                    switch (message.Text)
                    {
                        //Register a new form request
                        case StartCommand.REGISTER_MSG:
                            var pair = _stagesById.FirstOrDefault(x => 
                                update.Message.From != null && x.Key.UserId == userId);

                            var form = pair.Key;
                            if (form == null)
                            {
                                form = new RegisterFormAction(botClient, message.From.Id);
                                _stagesById.Add(form, new FormDataModel());

                                _stagesById.TryGetValue(form, out var dataModel);
                                    
                               await form.InitForm(dataModel, update);
                            }
                            break;
                        
                        //Contact us request
                        case StartCommand.CONTACT_US_MSG:
                            await botClient.SendTextMessageAsync(message.Chat, 
                                "🕔 Графік роботи: 09:00 - 22:00 (пн-пт) \n \n" +
                                "☎ Контактні номери: +380324931354 | +380324931352 \n \n" +
                                "📨 Email-адреса: dzvinochky2008@ukr.net", 
                                replyMarkup: new
                                    InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Перейти на сайт 🌎", SITE_URL)),
                                cancellationToken: cancellationToken);
                            break;
                        
                        //Check events request
                        case StartCommand.CHECK_EVENTS_MSG:
                            break;
                        
                        //Show on map request
                        case StartCommand.SHOW_ON_MAP_MSG:
                            await botClient.SendTextMessageAsync(message.Chat,
                                "Ми знаходимося за адресою: проспект Шевченка, 15", 
                                cancellationToken: cancellationToken);
                            
                            await botClient.SendLocationAsync(message.Chat, 50.3945274d, 24.2401365d, 
                                cancellationToken: cancellationToken);
                            break;
                    }

                    if (_stagesById.Any(x => message.From 
                        != null && x.Key.UserId == userId))
                    {
                        var key = _stagesById.FirstOrDefault(x => 
                            update.Message.From != null && x.Key.UserId == userId).Key;

                        await key.ProcessNextStage(_stagesById[key], update);
                    }
                }
            }
        }

        private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is ApiRequestException apiRequestException)
            {
                throw exception;
            }
        }
    }
}