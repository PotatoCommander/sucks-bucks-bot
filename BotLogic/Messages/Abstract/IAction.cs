using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages.Abstract
{
    public interface IAction
    {
        void SendMessage(MessageEventArgs ev, ITelegramBotClient bot);
    }
}