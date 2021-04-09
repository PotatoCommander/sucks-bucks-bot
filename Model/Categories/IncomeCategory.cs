using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sucks_bucks_bot.Model
{
    class IncomeCategory: Entity
    {
        public string CategoryName;
        public string Aliases;
        public int? UserId = null;
    }
}
