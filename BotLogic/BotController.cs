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
        private const string TOKEN = "1658228507:AAEF11ujKdslj3MLs-opP-2vKWQMJCiO79M";
        private ITelegramBotClient _bot;
        private const string FILE_NAME = @"C:\Jsones\CATS.json";
        private MessageFactory messageFactory;
        
        // private IncomeRepository _incomes;

        public BotController()
        {
            _bot = new TelegramBotClient(TOKEN) {Timeout = TimeSpan.FromSeconds(60)};
            messageFactory = new MessageFactory(_bot)
            {
                _expenses =  new ExpenseRepository(),
                _incomes = new IncomeRepository(),
                _users =  new UserRepository(),
                _expenseCategories =  new ExpenseCategoryRepository(),
                _incomeCategoryRepository = new IncomeCategoryRepository()
                
            };
            //_bot.GetUpdatesAsync();
            //_bot.StopReceiving();
            //ParseJson();
            _bot.OnMessage += MessageOnGet;
            _bot.MessageOffset = -1;
            _bot.StartReceiving();
        }

        public void MessageOnGet(object sender, MessageEventArgs ev)
        {
            var message = messageFactory.ShowMessageByCommand(ev);
            message.SendMessage(ev, _bot);
        }
        
        private List<ExpenseCategory> GenerateCategories(List<CategoryJSON> categories)
        {
            var list = new List<ExpenseCategory>();
            foreach (var item in categories)
            {
                list.Add(new ExpenseCategory()
                {
                    CategoryName = item.categoryName,
                    Aliases = item.aliases,
                    IsBaseExpense = item.isBaseExpense,
                    Id = Guid.NewGuid().GetHashCode()
                });
            }

            return list;
        }
    }
}