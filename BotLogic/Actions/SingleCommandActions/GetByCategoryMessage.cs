using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;
using sucks_bucks_bot.Repository.CategoryRepos;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages
{
    public class GetByCategoryAction: ActionWithDbInteract
    {
        public GetByCategoryAction(DbFacade dbFacade) : base(dbFacade)
        {
        }

        public override void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            var list = RepoFacade.expenses.GetAllExpensesOfUser(ev.Message.From.Id);
            var cats = RepoFacade.expenseCategories.GetAll();
            var str = "";
            var total = 0f;
            foreach (var item in cats)
            {
                var listOfExpenses = RepoFacade.expenses.GetByCategoryId(item.Id, list);
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