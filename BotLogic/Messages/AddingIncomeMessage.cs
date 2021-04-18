using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Repository;
using sucks_bucks_bot.Repository.CategoryRepos;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages
{
    public class AddingIncomeMessage:IAction
    {
        private IncomeRepository _incomeRepository;
        private IncomeCategoryRepository _incomeCategoryRepository;

        public AddingIncomeMessage(IncomeRepository incomeRepository, IncomeCategoryRepository incomeCategoryRepository)
        {
            _incomeRepository = incomeRepository;
            _incomeCategoryRepository = incomeCategoryRepository;
        }

        public void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            var inc = MessageParser.ParseIncomeMessage(ev.Message.Text, ev.Message.From, _incomeCategoryRepository);
            _incomeRepository.Insert(inc);
            bot.SendTextMessageAsync(ev.Message.Chat.Id,
                $"Добавлен доход {inc.Amount}руб в категорию {inc.Definition}");
        }
    }
}