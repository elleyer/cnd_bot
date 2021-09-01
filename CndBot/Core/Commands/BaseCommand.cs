using Telegram.Bot;
using Telegram.Bot.Types;

namespace CndBot.Core
{
    public abstract class BaseCommand : IBotCommand
    {
        public abstract void ExecuteCommand(ITelegramBotClient botClient, Update update);
    }
}