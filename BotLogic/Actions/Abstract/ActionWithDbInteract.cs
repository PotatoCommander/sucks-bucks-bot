using sucks_bucks_bot.Model;

namespace sucks_bucks_bot.BotLogic.Messages.Abstract
{
    public abstract class ActionWithDbInteract: Action
    {
        protected DbFacade RepoFacade;

        protected ActionWithDbInteract(DbFacade repoFacade)
        {
            RepoFacade = repoFacade;
        }
    }
}