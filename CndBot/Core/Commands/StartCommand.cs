using Telegram.Bot;
using Telegram.Bot.Types;

namespace CndBot.Core
{
    public class StartCommand : BaseCommand
    {
        private const string WELCOME_MSG = "Ласкаво просимо до офіційного телеграм-боту Народного Дому " +
                                           "міста Червоноград. Ви можете переглянути список доступних команд, " +
                                           "натиснувши відповідну кнопку на панелі, розташованій під цим " +
                                           "повідомленням.";
        public override async void ExecuteCommand(ITelegramBotClient botClient, Update update)
        {
            if (update.Message != null)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat, WELCOME_MSG);
            }
        }
    }
}