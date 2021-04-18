using System.Collections.Generic;
using sucks_bucks_bot.Model;

namespace sucks_bucks_bot.Repository.Abstractions
{
    public interface IRepository<T> where T: Entity
    {
        T GetById(int? id);
        List<T> GetAll();
        bool Update(T entity);
        bool Insert(T entity);
        bool Delete(T entity);
    }
}
