using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CndBot.Core
{
    public abstract class BaseCommand : IBotCommand
    {
        public abstract Task ExecuteCommand(ITelegramBotClient botClient, Update update);
    }
}