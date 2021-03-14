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
    class ExpenseRepository : AbstractRepo<Expense>, IRepository<Expense>
    {
        public bool Delete(Expense entity)
        {
            CreateConnection();
            var com = new SqlCommand("ExpensesDelete", connection);

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

        public List<Expense> GetAll()
        {
            CreateConnection();
            var ExpensesList = new List<Expense>();

            var com = new SqlCommand("ExpensesSelect", connection);
            com.CommandType = CommandType.StoredProcedure;
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            connection.Open();
            dataAdapter.Fill(dataTable);
            connection.Close();

            foreach (DataRow dr in dataTable.Rows)
            {
                ExpensesList.Add(
                    new Expense()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Amount = Convert.ToInt32(dr["amount"]),
                        CreatedTime = Convert.ToDateTime(dr["created"]),
                        CategoryId = Convert.ToInt32(dr["category_Id"]),
                        UserId = Convert.ToInt32(dr["User_Id"])
                    }
                    );
            }

            return ExpensesList;
        }

        public Entity GetById(int id)
        {
            return base.GetById(id, GetAll());
        }

        public bool Insert(Expense entity)
        {
            CreateConnection();
            var command = new SqlCommand("ExpensesInsert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@amount", entity.Amount);
            command.Parameters.AddWithValue("@created", entity.CreatedTime);
            command.Parameters.AddWithValue("@category_Id", entity.CategoryId);
            command.Parameters.AddWithValue("@User_Id", entity.UserId);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            if (i >= 1)
            {
                return true;
            }
            return false;
        }

        public bool Update(Expense entity)
        {
            CreateConnection();
            var command = new SqlCommand("ExpensesUpdate", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@amount", entity.Amount);
            command.Parameters.AddWithValue("@created", entity.CreatedTime);
            command.Parameters.AddWithValue("@category_Id", entity.CategoryId);
            command.Parameters.AddWithValue("@User_Id", entity.UserId);

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
