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
            var com = new SqlCommand("ExpensesDelete", connection) {CommandType = CommandType.StoredProcedure};

            com.Parameters.AddWithValue("@Id", entity.Id);

            connection.Open();
            var result = com.ExecuteNonQuery();
            connection.Close();
            return result >= 1;
        }

        public List<Expense> GetAll()
        {
            CreateConnection();
            var expensesList = new List<Expense>();

            var com = new SqlCommand("ExpensesSelect", connection) {CommandType = CommandType.StoredProcedure};
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            connection.Open();
            dataAdapter.Fill(dataTable);
            connection.Close();

            foreach (DataRow dr in dataTable.Rows)
            {
                expensesList.Add(
                    new Expense()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Amount = Convert.ToInt32(dr["amount"]),
                        CreatedTime = Convert.ToDateTime(dr["created"]),
                        CategoryId = Convert.ToInt32(dr["category_Id"]),
                        UserId = Convert.ToInt32(dr["User_Id"]),
                        NameOfExpense = Convert.ToString(dr["name_of_expense"])
                    }
                    );
            }

            return expensesList;
        }

        public Expense GetById(int id)
        {
            return base.GetById(id, GetAll());
        }
        public List<Expense> GetByCategoryId(int categoryId, List<Expense> list)
        {
            var listToReturn = new List<Expense>();
            foreach (var item in list)
            {
                if (item.CategoryId == categoryId)
                {
                    listToReturn.Add(item);
                }
            }
            return listToReturn;
        }

        public List<Expense> GetFirstByTime(int number, List<Expense> list)
        {
            var listToReturn = new List<Expense>();
            list = (list.OrderBy(x => x.CreatedTime).ToList());
            if (number > list.Count)
            {
                number = list.Count;
            }
            for (var i = 0; i < number; i++)
            {
                listToReturn.Add(list[i]);
            }
            return listToReturn;
        }
        public List<Expense> GetAllOfUser(int userId)
        {
            var listToReturn = new List<Expense>();
            var list = GetAll();
            foreach (var item in list)
            {
                if (item.UserId == userId)
                {
                    listToReturn.Add(item);
                }
            }
            listToReturn = (listToReturn.OrderBy(x => x.CreatedTime).ToList());
            return listToReturn;
        }

        public bool Insert(Expense entity)
        {
            CreateConnection();
            var command = new SqlCommand("ExpensesInsert", connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@amount", entity.Amount);
            command.Parameters.AddWithValue("@created", entity.CreatedTime);
            command.Parameters.AddWithValue("@category_Id", entity.CategoryId);
            command.Parameters.AddWithValue("@User_Id", entity.UserId);
            command.Parameters.AddWithValue("@name_of_expense", entity.NameOfExpense);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            return i >= 1;
        }

        public bool Update(Expense entity)
        {
            CreateConnection();
            var command = new SqlCommand("ExpensesUpdate", connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@amount", entity.Amount);
            command.Parameters.AddWithValue("@created", entity.CreatedTime);
            command.Parameters.AddWithValue("@category_Id", entity.CategoryId);
            command.Parameters.AddWithValue("@User_Id", entity.UserId);
            command.Parameters.AddWithValue("@name_of_expense", entity.NameOfExpense);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            return i >= 1;
        }
    }
}
