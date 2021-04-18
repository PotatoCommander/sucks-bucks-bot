using sucks_bucks_bot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sucks_bucks_bot.Repository.Abstractions;

namespace sucks_bucks_bot.Repository
{
    public class UserRepository : AbstractRepo<User>, IGenericRepository<User>
    {
        public bool Delete(User entity)
        {
            CreateConnection();
            var com = new SqlCommand("DeleteUserById", Connection) {CommandType = CommandType.StoredProcedure};

            com.Parameters.AddWithValue("@Id", entity.Id);

            Connection.Open();
            var result = com.ExecuteNonQuery();
            Connection.Close();
            if (result >= 1)
            {
                return true;
            }
            return false;
        }

        public List<User> GetAll()
        {
            CreateConnection();
            var usersList = new List<User>();

            var com = new SqlCommand("SelectAllUsers", Connection) {CommandType = CommandType.StoredProcedure};
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            Connection.Open();
            dataAdapter.Fill(dataTable);
            Connection.Close();

            foreach (DataRow dr in dataTable.Rows)
            {
                usersList.Add(
                    new User()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Username = Convert.ToString(dr["Username"])
                    }
                    );
            }

            return usersList;
        }

        public User GetById(int? id)
        {
            return base.GetById(id, GetAll());
        }

        public bool Insert(User entity)
        {
            CreateConnection();
            var command = new SqlCommand("InsertUser", Connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@username", entity.Username);

            Connection.Open();
            var i = command.ExecuteNonQuery();
            Connection.Close();

            return i >= 1;
        }

        public bool Update(User entity)
        {
            CreateConnection();
            var command = new SqlCommand("UpdateUser", Connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@username", entity.Username);

            Connection.Open();
            var i = command.ExecuteNonQuery();
            Connection.Close();

            return i >= 1;
        }
    }
}
