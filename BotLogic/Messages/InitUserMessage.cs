using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Repository;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using User = sucks_bucks_bot.Model.User;

namespace sucks_bucks_bot.BotLogic.Messages
{
    class InitUserAction: IAction
    {
        private UserRepository _users;

        private const string IMG_URL =
            "https://i.imgur.com/p45CIsK.png";

        public InitUserAction(UserRepository users)
        {
            _users = users;
        }

        public void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            bot.SendTextMessageAsync(ev.Message.Chat.Id, "Первый вход.----------------");
            bot.SendPhotoAsync(ev.Message.Chat.Id, new InputMedia(IMG_URL),
                "Это ваш первый вход в Sucks-bucks bot. \n" +
                "Используйте команду /start чтобы начать работу.");
            InitUser(ev);
        }
        private bool InitUser(MessageEventArgs ev)
        {
            try
            {
                _users.Insert(new User() {Id = ev.Message.From.Id, Username = ev.Message.From.FirstName});
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}