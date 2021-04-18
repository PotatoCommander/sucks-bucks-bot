using System;
namespace sucks_bucks_bot.Model
{
    public class Income: Transaction
    {
        public int CategoryId;
        public DateTime ExpiresAt;
        public override string ToString()
        {
            return $"\U00002795 {Amount} {Definition} созд: {CreatedTime} истекает: {ExpiresAt}";
        }
    }
}
