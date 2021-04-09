using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sucks_bucks_bot.Model
{
    abstract class Transaction: Entity
    {
        public float Amount;
        public DateTime CreatedTime;
        public int UserId;
    }
}
