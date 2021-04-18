namespace sucks_bucks_bot.Model
{
    public class Expense: Transaction
    {
        public int CategoryId ;
        public override string ToString()
        {
            return $"\U0000274c{Amount} {Definition} {CreatedTime.ToShortDateString()}" +
                   $" {CreatedTime.ToShortTimeString()}";
        }
    }
}
