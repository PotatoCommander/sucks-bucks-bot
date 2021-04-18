using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Repository;
using sucks_bucks_bot.Repository.CategoryRepos;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages
{
    public class GetByCategoryAction: IAction
    {
        private ExpenseCategoryRepository _expenseCategoryRepository;
        private ExpenseRepository _expenseRepository;

        public GetByCategoryAction(ExpenseRepository expenseRepository, ExpenseCategoryRepository expenseCategoryRepository)
        {
            _expenseRepository = expenseRepository;
            _expenseCategoryRepository = expenseCategoryRepository;
        }
        public void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            var list = _expenseRepository.GetAllExpensesOfUser(ev.Message.From.Id);
            var cats = _expenseCategoryRepository.GetAll();
            var str = "";
            var total = 0f;
            foreach (var item in cats)
            {
                var listOfExpenses = _expenseRepository.GetByCategoryId(item.Id, list);
                if (listOfExpenses.Count <= 0) continue;
                str = string.Concat(str, "Kатегория: [" + item.CategoryName + "]\n");
                var sum = 0f;
                foreach (var expense in listOfExpenses)
                {
                    sum += expense.Amount;
                    str = string.Concat(str,
                        expense.CreatedTime + "  " + expense.Definition + " " + expense.Amount + "р " + "\n");
                }

                total += sum;
                str = string.Concat(str, "Расходов в категории [" + item.CategoryName + "] " + sum + "р \n\n");
            }

            str = string.Concat(str, "Расходов всего." + total + "р \n\n");
            bot.SendTextMessageAsync(ev.Message.Chat.Id, str);
        }
    }
}