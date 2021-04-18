namespace sucks_bucks_bot.Model
{
    public class Budget: Entity
    {
        public float MonthlyBudget = 0;
        public float WeeklyBudget = 0;
        public float DailyBudget = 0;
        public string Comment = "";
        public int? UserId = null;
    }
}
