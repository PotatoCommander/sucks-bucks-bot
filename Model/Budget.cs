using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sucks_bucks_bot.Model
{
    class Budget: Entity
    {
        public int MonthlyBudget = 0;
        public int WeeklyBudget = 0;
        public int DailyBudget = 0;
        public string Comment = "";
        public int UserId = 0;
    }
}
