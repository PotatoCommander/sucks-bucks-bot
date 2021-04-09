using Newtonsoft.Json;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic
{
    internal class CategoryJSON
    {
        public string categoryName;
        public string aliases;
        public bool isBaseExpense;
    }

    internal class BotController
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

        private const string TOKEN = "1658228507:AAEF11ujKdslj3MLs-opP-2vKWQMJCiO79M";
        private ITelegramBotClient _bot;
        private const string FILE_NAME = @"C:\Jsones\CATS.json";

        private BudgetRepository _budgets;
        private ExpenseCategoryRepository _categories;
        private ExpenseRepository _expenses;
        private UserRepository _users;

        private string _currentCommand = "";

        public BotController()
        {
            _budgets = new BudgetRepository();

            _categories = new ExpenseCategoryRepository();

            _expenses = new ExpenseRepository();
            _users = new UserRepository();

            _bot = new TelegramBotClient(TOKEN) {Timeout = TimeSpan.FromSeconds(60)};
            //_bot.GetUpdatesAsync();
            //_bot.StopReceiving();
            ParseJson();
            _bot.OnMessage += MessageOnGet;
            _bot.StartReceiving();
        }

        public void MessageOnGet(object sender, MessageEventArgs ev)
        {
            if (_users.GetById(ev.Message.From.Id) == null) InitUser(ev);
            switch (ev.Message.Text?.ToLowerInvariant())
            {
                case "/start":
                    StartMessage(ev);
                    break;
                case "/getlast":
                    Get10LastMessage(ev);
                    break;
                case "/getall":
                    GetAllMessage(ev);
                    break;
                case "/bycategory":
                    GetByCategoryMessage(ev);
                    break;
                case "/setup_budget":
                    break;
                default:
                    if (IsExpenseString(ev.Message.Text))
                    {
                        AddingExpenseMessage(ev);
                    }
                    else
                    {
                        WrongInputMessage(ev);
                    }

                    break;
            }
        }

        private void StartMessage(MessageEventArgs ev)
        {
            //InitUser(ev);

            _bot.SendTextMessageAsync(ev.Message.Chat.Id,
                "Вас приветсвует чат бот для учета расходов!\n" + "Вот список команд:");
            var list = "";
            foreach (var str in _commands)
            {
                list = string.Concat(list, str + "\n\n");
            }

            list += "Чтобы добавить трату, напишите расход  в формате: 10 такси";
            _bot.SendTextMessageAsync(ev.Message.Chat.Id, list);
        }

        private void WrongInputMessage(MessageEventArgs ev)
        {
            _bot.SendTextMessageAsync(ev.Message.Chat.Id, "Это не похоже на строку расходов или команду :(");
        }

        private void AddingExpenseMessage(MessageEventArgs ev)
        {
            var exp = ParseMessage(ev.Message.Text, ev.Message.From);
            _expenses.Insert(exp);
            _bot.SendTextMessageAsync(ev.Message.Chat.Id,
                $"Добавлен расход {exp.Amount}руб в категорию {exp.NameOfExpense}");
        }

        private void Get10LastMessage(MessageEventArgs ev)
        {
            var list = _expenses.GetFirstByTime(10, _expenses.GetAllOfUser(ev.Message.From.Id));
            var str = "Последние 10 расходов:\n";
            foreach (var item in list)
            {
                var cat = _categories.GetById(item.CategoryId);
                str = string.Concat(str,
                    item.CreatedTime + "  " + item.NameOfExpense + " " + cat.CategoryName + " " + item.Amount + " " +
                    "\n");
            }

            _bot.SendTextMessageAsync(ev.Message.Chat.Id, str);
        }

        private void GetAllMessage(MessageEventArgs ev)
        {
            var list = _expenses.GetAllOfUser(ev.Message.From.Id);
            var str = "Все расходы:\n";
            foreach (var item in list)
            {
                var cat = _categories.GetById(item.CategoryId);
                str = string.Concat(str,
                    item.CreatedTime + "  " + item.NameOfExpense + " " + cat.CategoryName + " " + item.Amount + " " +
                    "\n");
            }

            _bot.SendTextMessageAsync(ev.Message.Chat.Id, str);
        }

        private void GetByCategoryMessage(MessageEventArgs ev)
        {
            var list = _expenses.GetAllOfUser(ev.Message.From.Id);
            var cats = _categories.GetAll();
            var str = "";
            var total = 0f;
            foreach (var item in cats)
            {
                var listOfExpenses = _expenses.GetByCategoryId(item.Id, list);
                if (listOfExpenses.Count <= 0) continue;
                str = string.Concat(str, "Kатегория: [" + item.CategoryName + "]\n");
                var sum = 0f;
                foreach (var expense in listOfExpenses)
                {
                    sum += expense.Amount;
                    str = string.Concat(str,
                        expense.CreatedTime + "  " + expense.NameOfExpense + " " + expense.Amount + "р " + "\n");
                }

                total += sum;
                str = string.Concat(str, "Расходов в категории [" + item.CategoryName + "] " + sum + "р \n\n");
            }

            str = string.Concat(str, "Расходов всего." + total + "р \n\n");
            _bot.SendTextMessageAsync(ev.Message.Chat.Id, str);
        }

        private void SetupBudgetMessage(MessageEventArgs ev)
        {
        }

        private Expense ParseMessage(string str, Telegram.Bot.Types.User user)
        {
            var inputWithoutSpaces = Regex.Replace(str, @"\s+", "");
            inputWithoutSpaces = inputWithoutSpaces.Replace(".", ",");

            var digitInputValue = Regex.Match(inputWithoutSpaces, @"[0-9]+(\,[0-9]+)?").Value;
            inputWithoutSpaces = inputWithoutSpaces.Replace(digitInputValue, "");
            var category = Regex.Match(inputWithoutSpaces, @"\D+").Value;
            var money = (float)Convert.ToDouble(digitInputValue, CultureInfo.CurrentCulture);

            var foundedCategory = _categories.GetByString(category);
            return (new Expense()
            {
                Amount = money,
                CreatedTime = DateTime.Now,
                CategoryId = foundedCategory.Id,
                Id = Guid.NewGuid().GetHashCode(),
                UserId = user.Id,
                NameOfExpense = category
            });
        }

        private List<ExpenseCategory> GenerateCategories(List<CategoryJSON> categories)
        {
            var list = new List<ExpenseCategory>();
            foreach (var item in categories)
            {
                list.Add(new ExpenseCategory()
                {
                    CategoryName = item.categoryName,
                    Aliases = item.aliases,
                    IsBaseExpense = item.isBaseExpense,
                    Id = Guid.NewGuid().GetHashCode()
                });
            }

            return list;
        }

        public List<CategoryJSON> ParseJson()
        {
            var deserialized = JsonConvert.DeserializeObject<List<CategoryJSON>>(File.ReadAllText(FILE_NAME));

            return deserialized;
        }

        public void ToJson(List<CategoryJSON> categories)
        {
            var serialized = JsonConvert.SerializeObject(categories,
                new JsonSerializerSettings {Formatting = Formatting.Indented});
            File.WriteAllTextAsync(FILE_NAME, serialized);
        }

        private bool IsExpenseString(string str)
        {
            var patternFloat = @"^\d+.\d \w";
            var patternInt = @"^\d+ \w";
            return (Regex.IsMatch(str, patternFloat) || Regex.IsMatch(str, patternInt));
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