using Telegram.Bot;
using Telegram.Bot.Types;

namespace CndBot.Core
{
    public class GetInfoCommand : BaseCommand
    {
        private const string INFO_MSG =    "Це просто тестове повідомлення про інфу";
        public override async void ExecuteCommand(ITelegramBotClient botClient, Update update)
        {
            if (update.Message != null)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat, INFO_MSG);
            }
        }
    }
}