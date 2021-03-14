using Newtonsoft.Json;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace sucks_bucks_bot.BotLogic
{
	class CategoryJSON
	{
		public string categoryName;
		public string aliases;
		public bool isBaseExpense;
	}
	class BotController
	{
		private List<string> commands = new List<string>()
		{
			"/start - Стартовое меню",
			"/getlast - Получить 10 последних расходов",
			"/getall - Получить все расходы.",
			"/bycategory - Сумма расходов по категориям за месяц."

		};
		const string token = "1658228507:AAEF11ujKdslj3MLs-opP-2vKWQMJCiO79M";
		ITelegramBotClient _bot;
		private string fileName = @"C:\Jsones\CATS.json";

		BudgetRepository budgetes;
		CategoryRepository categories;
		ExpenseRepository expenses;
		UserRepository users;

		private string currentCommand;

		public BotController()
		{
			budgetes = new BudgetRepository();

			categories = new CategoryRepository();

			expenses = new ExpenseRepository();
			users = new UserRepository();

			_bot = new TelegramBotClient(token) { Timeout = TimeSpan.FromSeconds(60) };
			//_bot.GetUpdatesAsync();
			//_bot.StopReceiving();
			ParseJson();
			_bot.OnMessage += MessageOnGet;
			_bot.StartReceiving();
		}
		public void MessageOnGet(object sender, MessageEventArgs ev)
		{
			if (users.GetById(ev.Message.From.Id) == null) InitUser(ev);
			switch (ev.Message.Text?.ToLowerInvariant())
			{
				case "/start":
					StartMessage(ev);
					break;
				case "/getlast":
					GetLastMessage(ev);
					break;
				case "/getall":
					GetAllMessage(ev);
					break;
				case "/bycategory":
					GetByCategoryMessage(ev);
					break;
				default:
					if (isExpenseString(ev.Message.Text))
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

			_bot.SendTextMessageAsync(ev.Message.Chat.Id, "Вас приветсвует чат бот для учета расходов!\n" +
				"Вот список команд:");
			string list = "";
			foreach (var str in commands)
			{
				list = string.Concat(list, str + "\n\n");
			}
			list += "Чтобы добавить трату, напишите расход в формате: 10 такси";
			_bot.SendTextMessageAsync(ev.Message.Chat.Id, list);
		}
		private void WrongInputMessage(MessageEventArgs ev)
		{
			_bot.SendTextMessageAsync(ev.Message.Chat.Id, "Это не похоже на строку расходов или команду :(");
		}
		private void AddingExpenseMessage(MessageEventArgs ev)
        {
			var exp = ParseMessage(ev.Message.Text, ev.Message.From);
			expenses.Insert(exp);
			var cat = categories.GetById(exp.CategoryId);
			_bot.SendTextMessageAsync(ev.Message.Chat.Id, $"Добавлен расход {exp.Amount}руб в категорию {exp.nameOfExpense}");
		}
		private void GetLastMessage(MessageEventArgs ev)
        {
			var list = expenses.GetFirstByTime(10, expenses.GetAllOfUser(ev.Message.From.Id));
			var str = "Последние 10 расходов:\n";
			foreach (var item in list)
            {
				var cat = categories.GetById(item.CategoryId);
				str = string.Concat(str, item.CreatedTime.ToString() +"  " + item.nameOfExpense +" "
					+ cat.CategoryName + " " + item.Amount + " " + "\n");
			}
			_bot.SendTextMessageAsync(ev.Message.Chat.Id, str);
		}
		private void GetAllMessage(MessageEventArgs ev)
        {
			var list = expenses.GetAllOfUser(ev.Message.From.Id);
			var str = "Все расходы:\n";
			foreach (var item in list)
			{
				var cat = categories.GetById(item.CategoryId);
				str = string.Concat(str, item.CreatedTime.ToString() + "  " + item.nameOfExpense + " "
					+ cat.CategoryName + " " + item.Amount + " " + "\n");
			}
			_bot.SendTextMessageAsync(ev.Message.Chat.Id, str);
		}
		private void GetByCategoryMessage(MessageEventArgs ev)
        {
			var list = expenses.GetAllOfUser(ev.Message.From.Id);
			var cats = categories.GetAll();
			var str = "";
			var total = 0;
			foreach (var item in cats)
            {
				var listOfExpenses = expenses.GetByCategoryID(item.Id, list);
				if (listOfExpenses.Count > 0)
				{
					str = string.Concat(str, "Kатегория: [" + item.CategoryName + "]\n");
					int sum = 0;
					foreach (var expense in listOfExpenses)
					{
						sum += expense.Amount;
						str = string.Concat(str, expense.CreatedTime.ToString() + "  " + expense.nameOfExpense + " " + expense.Amount + "р " + "\n");
					}
					total += sum;
					str = string.Concat(str, "Расходов в категории [" + item.CategoryName + "] " + sum + "р \n\n");
				}
			}
			str = string.Concat(str, "Расходов всего." + total + "р \n\n");
			_bot.SendTextMessageAsync(ev.Message.Chat.Id, str);
		}
		private Expense ParseMessage(string str, Telegram.Bot.Types.User user)
		{
			var input = Regex.Replace(str, @"\s+", "");
			string digits = Regex.Match(input, @"^\d+").Value;
			string category = Regex.Match(input, @"\D+").Value;
			var money = Convert.ToInt32(digits);

			var findedCategory = categories.GetByString(category);
			return (new Expense()
			{
				Amount = money,
				CreatedTime = DateTime.Now,
				CategoryId = findedCategory.Id,
				Id = Guid.NewGuid().GetHashCode(),
				UserId = user.Id,
				nameOfExpense = category
			});
		}

		private List<Category> GenerateCategories(List<CategoryJSON> categories)
		{
			var list = new List<Category>();
			foreach (var item in categories)
			{
				list.Add(new Category()
				{
					CategoryName = item.categoryName,
					Aliases = item.aliases,
					isBaseExpense = item.isBaseExpense,
					Id = Guid.NewGuid().GetHashCode()
				});
			}
			return list;
		}
		public List<CategoryJSON> ParseJson()
		{
			var deserialized = JsonConvert.DeserializeObject<List<CategoryJSON>>(File.ReadAllText(fileName));

			return deserialized;
		}
		public void ToJson(List<CategoryJSON> categories)
		{
			var serialized = JsonConvert.SerializeObject(categories, new JsonSerializerSettings
			{
				Formatting = Formatting.Indented
			});
			File.WriteAllTextAsync(fileName, serialized);
		}
		private bool isExpenseString(string str)
		{
			string pattern = @"\d \w";
			return Regex.IsMatch(str, pattern);
		}
		private bool InitUser(MessageEventArgs ev)
		{
			var use = ev.Message.From;
			try
			{
				users.Insert(new User() { Id = ev.Message.From.Id, Username = ev.Message.From.FirstName });
				return true;
			}
			catch
			{
				return false;
			}
		}
		//private int getAmountAndCategory(out string str)
		//{

		//}
	}
}
