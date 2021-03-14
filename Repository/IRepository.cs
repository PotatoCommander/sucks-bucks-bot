using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sucks_bucks_bot.Model;

namespace sucks_bucks_bot.Repository
{
    interface IRepository<T> where T: Entity
    {
        Entity GetById(int id);
        List<T> GetAll();
        bool Update(T entity);
        bool Insert(T entity);
        bool Delete(T entity);
    }
}
