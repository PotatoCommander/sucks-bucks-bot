using System;
using sucks_bucks_bot.BotLogic.Messages;
using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;
using sucks_bucks_bot.Repository.CategoryRepos;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic
{
    public class MessageFactory
    {
        public BudgetRepository budgets { get; set; }
        public ExpenseCategoryRepository _expenseCategories { get; set; }
        public IncomeCategoryRepository _incomeCategoryRepository { get; set; }
        public ExpenseRepository _expenses { get; set; }
        public IncomeRepository _incomes { get; set; }
        public UserRepository _users { get; set; }

        public ITelegramBotClient _bot;

        public MessageFactory(ITelegramBotClient bot)
        {
            _bot = bot;
        }

        public IAction ShowMessageByCommand(MessageEventArgs ev)
        {
            if (_users.GetById(ev.Message.From.Id) == null) return new InitUserAction(_users);
            switch (ev.Message.Text?.ToLowerInvariant())
            {
                case "/start":
                    return new StartAction();
                case "/getlast":
                    return new GetLastAction(_expenses, _incomes);
                case "/getall":
                    return new GetAllTransactionsAction(_expenses);
                case "/bycategory":
                    return new GetByCategoryAction(_expenses, _expenseCategories);
                case "/setup_budget":
                default:
                    if (MessageParser.IsExpenseString(ev.Message.Text))
                    {
                        return new AddingExpenseMessage(_expenses, _expenseCategories);
                    }
                    
                    if (MessageParser.IsIncomeString(ev.Message.Text))
                    {
                        return new AddingIncomeMessage(_incomes, _incomeCategoryRepository);
                    }
                    
                    return new WrongInputMessage();
            }
            
        }

    }
}