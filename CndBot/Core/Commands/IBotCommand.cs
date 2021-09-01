using Telegram.Bot;
using Telegram.Bot.Types;

namespace CndBot.Core
{
    public interface IBotCommand
    { 
        void ExecuteCommand(ITelegramBotClient botClient, Update update);
    }
}