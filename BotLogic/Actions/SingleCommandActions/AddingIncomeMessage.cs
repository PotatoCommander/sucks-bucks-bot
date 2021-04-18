using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;
using sucks_bucks_bot.Repository.CategoryRepos;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages
{
    public class AddingIncomeMessage:ActionWithDbInteract
    {
        public override void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            var inc = MessageParser.ParseIncomeMessage(ev.Message.Text, ev.Message.From, RepoFacade.incomeCategoryRepository);
            RepoFacade.incomes.Insert(inc);
            bot.SendTextMessageAsync(ev.Message.Chat.Id,
                $"Добавлен доход {inc.Amount}руб в категорию {inc.Definition}");
        }

        public AddingIncomeMessage(DbFacade dbFacade) : base(dbFacade)
        {
        }
    }
}