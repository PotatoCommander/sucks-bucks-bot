using System.Collections.Generic;
using sucks_bucks_bot.BotLogic.Messages.Abstract;
using sucks_bucks_bot.Model;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic.Messages
{
    public class StartAction: Action
    {
        private readonly List<string> _commands = new List<string>()
        {
            "/start - Стартовое меню",
            "/getlast - Получить 10 последних расходов",
            "/getall - Получить все расходы.",
            "/bycategory - Сумма расходов по категориям за месяц.",
            "/getcats - Информация о категориях и их синонимы",
            "/getbudget - информация о бюджете",
            "/setincome - Добавить прибыль"
        };

        public override void SendMessage(MessageEventArgs ev, ITelegramBotClient bot)
        {
            bot.SendTextMessageAsync(ev.Message.Chat.Id,
                "Вас приветсвует чат бот для учета расходов!\n" + "Вот список команд:");
            var list = "";
            foreach (var str in _commands)
            {
                list = string.Concat(list, str + "\n\n");
            }

            list += "Чтобы добавить транзакцию, напишите расход  в формате: -10 такси +102.50 зарплата";
            bot.SendTextMessageAsync(ev.Message.Chat.Id, list);
        }
    }
}