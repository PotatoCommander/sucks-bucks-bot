using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sucks_bucks_bot.Model
{
    class Budget: Entity
    {
        public float MonthlyBudget = 0;
        public float WeeklyBudget = 0;
        public float DailyBudget = 0;
        public string Comment = "";
    }
}
