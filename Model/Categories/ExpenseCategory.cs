namespace sucks_bucks_bot.Model
{
    public class ExpenseCategory: Entity
    {
        public string CategoryName;
        public string Aliases;
        public bool IsBaseExpense;
        public int? UserId = null;
    }
}
