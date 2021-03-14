using sucks_bucks_bot.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sucks_bucks_bot.Repository
{
    abstract class AbstractRepo<T> where T: Entity
    {
        protected SqlConnection connection;
        protected string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\LENOVO\SOURCE\REPOS\SUCKS-BUCKS-BOT\SBBOTDB.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        protected void CreateConnection()
        {
            var constr = connectionString;
            connection = new SqlConnection(constr);
        }
        protected T GetById(int id, List<T> list)
        {
            foreach (var item in list)
            {
                if (item.Id == id) return item;
            }
            return null;
        }
    }
}
