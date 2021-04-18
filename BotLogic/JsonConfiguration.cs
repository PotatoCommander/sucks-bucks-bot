using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using sucks_bucks_bot.Model;

namespace sucks_bucks_bot.BotLogic
{
    public class CategoryJSON
    {
        public string categoryName;
        public string aliases;
        public bool isBaseExpense;
    }
    public class JsonConfigure
    {
        private string _filepath;

        JsonConfigure(string filepath)
        {
            _filepath = filepath;
        }
        public List<CategoryJSON> ParseJson()
        {
            var deserialized = JsonConvert.DeserializeObject<List<CategoryJSON>>(File.ReadAllText(_filepath));

            return deserialized;
        }

        public void ToJson(List<CategoryJSON> categories)
        {
            var serialized = JsonConvert.SerializeObject(categories,
                new JsonSerializerSettings {Formatting = Formatting.Indented});
            File.WriteAllTextAsync(_filepath, serialized);
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
    }
}