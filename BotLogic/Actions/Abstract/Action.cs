using sucks_bucks_bot.Model;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages.Abstract
{
    public abstract class Action
    {
        public abstract void SendMessage(MessageEventArgs ev, ITelegramBotClient bot);
    }
}