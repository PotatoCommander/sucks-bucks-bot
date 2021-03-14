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
            var bot = new BotController();

            Console.ReadKey();

        }
    }
}
