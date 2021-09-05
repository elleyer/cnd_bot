using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CndBot.Core
{
    public class StartCommand : BaseCommand
    {
        public const string REGISTER_MSG = "Зареєструватися на гурток";
        public const string CHECK_EVENTS_MSG = "Майбутні події";
        public const string CONTACT_US_MSG = "Зворотній зв'язок";
        public const string SHOW_ON_MAP_MSG = "Показати на карті 📍";
        
        private const string WELCOME_MSG = "Ласкаво просимо до офіційного телеграм-боту Народного Дому " +
                                           "міста Червоноград. Що Вас цікавить?";
        
        public override async void ExecuteCommand(ITelegramBotClient botClient, Update update)
        {
            if (update.Message != null)
            {   
                var keyboard = new ReplyKeyboardMarkup(new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>
                    {
                        new KeyboardButton(REGISTER_MSG),
                        new KeyboardButton(CHECK_EVENTS_MSG)
                    },
                    new List<KeyboardButton>
                    {
                        new KeyboardButton(CONTACT_US_MSG),
                        new KeyboardButton(SHOW_ON_MAP_MSG)
                    }
                });
                
                keyboard.ResizeKeyboard = true;
                
                await botClient.SendTextMessageAsync(update.Message.Chat, WELCOME_MSG, replyMarkup: keyboard);
            }
        }
    }
}