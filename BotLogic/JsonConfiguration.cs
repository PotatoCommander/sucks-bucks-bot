using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using sucks_bucks_bot.Model;

namespace sucks_bucks_bot.BotLogic
{
    public class CategoryJson
    {
        public string CategoryName;
        public string Aliases;
        public bool IsBaseExpense;
    }
    public class JsonConfigure
    {
        private string _filepath;

        JsonConfigure(string filepath)
        {
            _filepath = filepath;
        }
        public List<CategoryJson> ParseJson()
        {
            var deserialized = JsonConvert.DeserializeObject<List<CategoryJson>>(File.ReadAllText(_filepath));

            return deserialized;
        }

        public void ToJson(List<CategoryJson> categories)
        {
            var serialized = JsonConvert.SerializeObject(categories,
                new JsonSerializerSettings {Formatting = Formatting.Indented});
            File.WriteAllTextAsync(_filepath, serialized);
        }
        private List<ExpenseCategory> GenerateCategories(List<CategoryJson> categories)
        {
            var list = new List<ExpenseCategory>();
            foreach (var item in categories)
            {
                list.Add(new ExpenseCategory()
                {
                    CategoryName = item.CategoryName,
                    Aliases = item.Aliases,
                    IsBaseExpense = item.IsBaseExpense,
                    Id = Guid.NewGuid().GetHashCode()
                });
            }

            return list;
        }
    }
}