using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sucks_bucks_bot.Model
{
    class ExpenseCategory: Entity
    {
        public string CategoryName;
        public string Aliases;
        public bool IsBaseExpense;
        public int? UserId = null;
    }
}
