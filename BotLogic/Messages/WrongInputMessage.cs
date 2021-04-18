using sucks_bucks_bot.BotLogic.Messages.Abstract;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages
{
    public class WrongInputMessage: IAction
    {
        public void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            bot.SendTextMessageAsync(ev.Message.Chat.Id, "Это не похоже на строку расходов или команду :(");
        }
    }
}