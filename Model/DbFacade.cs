using System.Collections.Generic;
using System.Reflection;
using sucks_bucks_bot.Repository;
using sucks_bucks_bot.Repository.Abstractions;
using sucks_bucks_bot.Repository.CategoryRepos;

namespace sucks_bucks_bot.Model
{
    public class DbFacade
    {
        public BudgetRepository budgets { get; set; }
        public ExpenseCategoryRepository expenseCategories { get; set; }
        public IncomeCategoryRepository incomeCategoryRepository { get; set; }
        public ExpenseRepository expenses { get; set; }
        public IncomeRepository incomes { get; set; }
        public UserRepository users { get; set; }
    }
}