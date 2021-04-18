using Newtonsoft.Json;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using sucks_bucks_bot.Repository.CategoryRepos;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic
{
    internal class BotController
    {
        private readonly ITelegramBotClient _bot;
        private const string FileName = @"C:\Jsones\CATS.json";
        private readonly MessageFactory _messageFactory;
        

        public BotController(string token)
        {
            _bot = new TelegramBotClient(token) {Timeout = TimeSpan.FromSeconds(60)};
            _messageFactory = new MessageFactory(_bot, new DbFacade());
            
            _bot.OnMessage += MessageOnGet;
            _bot.StartReceiving();
        }

        private void MessageOnGet(object sender, MessageEventArgs ev)
        {
            var message = _messageFactory.ShowMessageByCommand(ev);
            message.SendMessage(ev, _bot);
        }
        
        private List<ExpenseCategory> GenerateCategories(List<CategoryJson> categories)
        {
            var list = new List<ExpenseCategory>();
            foreach (var item in categories)
            {
                list.Add(new ExpenseCategory()
                {
                    CategoryName = item.CategoryName,
                    Aliases = item.Aliases,
                    IsBaseExpense = item.IsBaseExpense,
                    Id = Guid.NewGuid().GetHashCode()
                });
            }

            return list;
        }
    }
}