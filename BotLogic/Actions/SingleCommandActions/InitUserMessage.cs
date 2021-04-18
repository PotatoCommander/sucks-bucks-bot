using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using User = sucks_bucks_bot.Model.User;

namespace sucks_bucks_bot.BotLogic.Messages
{
    class InitUserAction: ActionWithDbInteract
    {
        private const string ImgUrl =
            "https://i.imgur.com/p45CIsK.png";


        public InitUserAction(DbFacade dbFacade) : base(dbFacade)
        {
        }

        public override void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            bot.SendTextMessageAsync(ev.Message.Chat.Id, "Первый вход.----------------");
            bot.SendPhotoAsync(ev.Message.Chat.Id, new InputMedia(ImgUrl),
                "Это ваш первый вход в Sucks-bucks bot. \n" +
                "Используйте команду /start чтобы начать работу.");
            InitUser(ev);
        }
        private bool InitUser(MessageEventArgs ev)
        {
            try
            {
                RepoFacade.users.Insert(new User() {Id = ev.Message.From.Id, Username = ev.Message.From.FirstName});
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}