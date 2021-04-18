using System;
using sucks_bucks_bot.BotLogic.Messages;
using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;
using sucks_bucks_bot.Repository.CategoryRepos;
using Telegram.Bot;
using Telegram.Bot.Args;
using Action = sucks_bucks_bot.BotLogic.Messages.Abstract.Action;

namespace sucks_bucks_bot.BotLogic
{
    public class MessageFactory
    {
        private ITelegramBotClient _bot;
        private DbFacade _dbFacade;

        public MessageFactory(ITelegramBotClient bot, DbFacade dbFacade)
        {
            _bot = bot;
            _dbFacade = dbFacade;
        }

        public Action ShowMessageByCommand(MessageEventArgs ev)
        {
            if (_dbFacade.users.GetById(ev.Message.From.Id) == null) return new InitUserAction(_dbFacade);
            switch (ev.Message.Text?.ToLowerInvariant())
            {
                case "/start":
                    return new StartAction();
                case "/getlast":
                    return new GetLastAction(_dbFacade);
                case "/getall":
                    return new GetAllTransactionsAction(_dbFacade);
                case "/bycategory":
                    return new GetByCategoryAction(_dbFacade);
                case "/setup_budget":
                default:
                    if (MessageParser.IsExpenseString(ev.Message.Text))
                    {
                        return new AddingExpenseMessage(_dbFacade);
                    }
                    
                    if (MessageParser.IsIncomeString(ev.Message.Text))
                    {
                        return new AddingIncomeMessage(_dbFacade);
                    }
                    
                    return new WrongInputMessage();
            }
            
        }

    }
}