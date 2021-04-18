using System.Collections.Generic;
using System.Linq;
using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;
using sucks_bucks_bot.Repository.CategoryRepos;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages
{
    public class GetLastAction: ActionWithDbInteract
    {
        public GetLastAction(DbFacade dbFacade) : base(dbFacade)
        {
        }

        public override void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            List<Transaction> list = new List<Transaction>(RepoFacade.expenses.GetAllExpensesOfUser(ev.Message.From.Id));
            list.AddRange(new List<Transaction>(RepoFacade.incomes.GetAllExpensesOfUser(ev.Message.From.Id))); 
            list = (list.OrderByDescending(x => x.CreatedTime).ToList());
            int boundary = 10;
            if (list.Count < 10)
            {
                boundary = list.Count;
            }
            list = list.GetRange(0, boundary);

            var str = "Последние 10 транзакций:\n";
            foreach (var item in list)
            {
                //var cat = _categories.GetById(item.CategoryId);
                str = string.Concat(str, item.ToString() +
                                         "\n");
            }

            bot.SendTextMessageAsync(ev.Message.Chat.Id, str);
        }
    }
}