using System.Collections.Generic;
using sucks_bucks_bot.Repository.Abstractions;

namespace sucks_bucks_bot.Model
{
    public class DbContext
    {
        private IEnumerable<IRepository<Entity>> _repositories;

        public DbContext(IEnumerable<IRepository<Entity>> repositories)
        {
            _repositories = repositories;
        }
    }
}