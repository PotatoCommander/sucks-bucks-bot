using sucks_bucks_bot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sucks_bucks_bot.Repository
{
    class UserRepository : AbstractRepo<User>, IRepository<User>
    {
        public bool Delete(User entity)
        {
            CreateConnection();
            var com = new SqlCommand("UsersDelete", connection);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", entity.Id);

            connection.Open();
            var result = com.ExecuteNonQuery();
            connection.Close();
            if (result >= 1)
            {
                return true;
            }
            return false;
        }

        public List<User> GetAll()
        {
            CreateConnection();
            var UsersList = new List<User>();

            var com = new SqlCommand("UsersSelect", connection);
            com.CommandType = CommandType.StoredProcedure;
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            connection.Open();
            dataAdapter.Fill(dataTable);
            connection.Close();

            foreach (DataRow dr in dataTable.Rows)
            {
                UsersList.Add(
                    new User()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Username = Convert.ToString(dr["Username"])
                    }
                    );
            }

            return UsersList;
        }

        public Entity GetById(int id)
        {
            return base.GetById(id, GetAll());
        }

        public bool Insert(User entity)
        {
            CreateConnection();
            var command = new SqlCommand("UsersInsert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@username", entity.Username);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            if (i >= 1)
            {
                return true;
            }
            return false;
        }

        public bool Update(User entity)
        {
            CreateConnection();
            var command = new SqlCommand("UsersUpdate", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@username", entity.Username);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            if (i >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
