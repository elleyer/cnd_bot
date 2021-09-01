using System;

namespace CndBot.Core
{
    public class CommandFactory
    {
        public static IBotCommand GetCommand(string query)
        {
            return query switch
            {
                "/start" => new StartCommand(),
                "/get_info" => new GetInfoCommand(),
                _ => throw new Exception($"Invalid command {query}")
            };
        }
    }
}