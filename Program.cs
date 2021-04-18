using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using sucks_bucks_bot.BotLogic;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;

namespace sucks_bucks_bot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            const string Token = "1658228507:AAEF11ujKdslj3MLs-opP-2vKWQMJCiO79M";
            var bot = new BotController(Token);

            Console.ReadKey();

        }
    }
}
