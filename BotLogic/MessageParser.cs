using System;
using System.Globalization;
using System.Text.RegularExpressions;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository.CategoryRepos;

namespace sucks_bucks_bot.BotLogic
{
    public static class MessageParser
    {
        public static Expense ParseExpenseMessage(string str, Telegram.Bot.Types.User user,
            ExpenseCategoryRepository expCat)
        {
            var inputWithoutSpaces = Regex.Replace(str, @"^\-\s+", "");
            inputWithoutSpaces = inputWithoutSpaces.Replace(".", ",");

            var digitInputValue = Regex.Match(inputWithoutSpaces, @"[0-9]+(\,[0-9]+)?").Value;
            inputWithoutSpaces = inputWithoutSpaces.Replace(digitInputValue, "");
            inputWithoutSpaces = inputWithoutSpaces.Replace(@"-", "");


            var category = Regex.Match(inputWithoutSpaces, @"\D+").Value;
            var money = (float) Convert.ToDouble(digitInputValue, CultureInfo.CurrentCulture);

            var foundedCategory = expCat.GetByString(category);
            return (new Expense()
            {
                Amount = money,
                CreatedTime = DateTime.Now,
                CategoryId = foundedCategory.Id,
                Id = Guid.NewGuid().GetHashCode(),
                UserId = user.Id,
                Definition = category
            });
        }

        public static Income ParseIncomeMessage(string str, Telegram.Bot.Types.User user,IncomeCategoryRepository incCat )
        {
            var inputWithoutSpaces = Regex.Replace(str, @"^\+\s+", "");
            inputWithoutSpaces = inputWithoutSpaces.Replace(".", ",");

            var digitInputValue = Regex.Match(inputWithoutSpaces, @"[0-9]+(\,[0-9]+)?").Value;
            inputWithoutSpaces = inputWithoutSpaces.Replace(digitInputValue, "");
            inputWithoutSpaces = inputWithoutSpaces.Replace(@"+", "");


            var category = Regex.Match(inputWithoutSpaces, @"\D+").Value;
            var money = (float) Convert.ToDouble(digitInputValue, CultureInfo.CurrentCulture);

            var foundedCategory = incCat.GetByString(category);
            return (new Income()
            {
                Amount = money,
                CreatedTime = DateTime.Now,
                CategoryId = 0, //TODO: fix category id
                Id = Guid.NewGuid().GetHashCode(),
                UserId = user.Id,
                Definition = category,
                ExpiresAt = new DateTime(2025, 7, 20)
            });
        }

        public static bool IsExpenseString(string str)
        {
            str = str.Replace(",", ".");
            var pattern = @"^\-\d*\.?\d+ \w+";
            return Regex.IsMatch(str, pattern);
        }

        public static bool IsIncomeString(string str)
        {
            str = str.Replace(",", ".");
            var pattern = @"^\+\d*\.?\d+ \w+";
            return Regex.IsMatch(str, pattern);
        }
    }
}