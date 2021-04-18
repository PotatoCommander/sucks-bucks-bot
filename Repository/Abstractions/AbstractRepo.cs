using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using sucks_bucks_bot.Model;

namespace sucks_bucks_bot.Repository.Abstractions
{
    public abstract class AbstractRepo<T> where T: Entity
    {
        protected SqlConnection connection;
        protected string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SBB_DB;Integrated Security=True";

        protected void CreateConnection()
        {
            var constr = connectionString;
            connection = new SqlConnection(constr);
        }
        protected T GetById(int? id, List<T> list)
        {
            return list.FirstOrDefault(item => item.Id == id);
        }
    }
}
