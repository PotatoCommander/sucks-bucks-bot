using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sucks_bucks_bot.Model
{
    class Expense: Entity
    {
        public int Amount;
        public DateTime CreatedTime;
        public int CategoryId;
        public int UserId;
    }
}
