using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages
{
    public class GetAllTransactionsAction: ActionWithDbInteract
    {
        public GetAllTransactionsAction(DbFacade dbFacade) : base(dbFacade)
        {
        }

        public override void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            var list = RepoFacade.expenses.GetAllExpensesOfUser(ev.Message.From.Id);
            var str = "Все расходы:\n";
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