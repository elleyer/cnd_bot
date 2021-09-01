using System;
using System.Threading;
using System.Threading.Tasks;
using CndBot.Core;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace CndBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            client.Init();

            Console.ReadKey();
        }
    }
}