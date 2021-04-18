using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;
using sucks_bucks_bot.Repository.CategoryRepos;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages
{
    public class AddingExpenseMessage: ActionWithDbInteract
    { 
        public AddingExpenseMessage(DbFacade repoFacade) : base(repoFacade){}
        public override void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            var exp = MessageParser.ParseExpenseMessage(ev.Message.Text, ev.Message.From, RepoFacade.expenseCategories);
            RepoFacade.expenses.Insert(exp);
            bot.SendTextMessageAsync(ev.Message.Chat.Id,
                $"Добавлен расход {exp.Amount}руб в категорию {exp.Definition}");
        }
    }
}