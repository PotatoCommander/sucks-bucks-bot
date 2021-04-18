using System;
namespace sucks_bucks_bot.Model
{
    public abstract class Transaction: Entity
    {
        public float Amount;
        public string Definition;
        public DateTime CreatedTime;
        public int UserId;
    }
}

