using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using sucks_bucks_bot.Model;

namespace sucks_bucks_bot.Repository.Abstractions
{
    public abstract class AbstractRepo<T> where T: Entity
    {
        protected SqlConnection Connection;
        protected string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SBB_DB;Integrated Security=True";

        protected void CreateConnection()
        {
            var constr = ConnectionString;
            Connection = new SqlConnection(constr);
        }
        protected T GetById(int? id, IEnumerable<T> list)
        {
            return list.FirstOrDefault(item => item.Id == id);
        }
    }
}
