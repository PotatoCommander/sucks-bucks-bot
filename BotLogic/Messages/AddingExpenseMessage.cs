using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Repository;
using sucks_bucks_bot.Repository.CategoryRepos;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages
{
    public class AddingExpenseMessage: IAction
    {
        private ExpenseRepository _expenseRepository;
        private ExpenseCategoryRepository _expenseCategoryRepository;

        public AddingExpenseMessage(ExpenseRepository expenseRepository, ExpenseCategoryRepository expenseCategoryRepository)
        {
            _expenseRepository = expenseRepository;
            _expenseCategoryRepository = expenseCategoryRepository;
        }
        public void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            var exp = MessageParser.ParseExpenseMessage(ev.Message.Text, ev.Message.From, _expenseCategoryRepository);
            _expenseRepository.Insert(exp);
            bot.SendTextMessageAsync(ev.Message.Chat.Id,
                $"Добавлен расход {exp.Amount}руб в категорию {exp.Definition}");
        }
    }
}