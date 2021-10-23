using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CndBot.Core
{
    public interface IBotCommand
    { 
        Task ExecuteCommand(ITelegramBotClient botClient, Update update);
    }
}