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
    public class ExpenseRepository : AbstractRepo<Expense>, IGenericRepository<Expense>
    {
        public bool Delete(Expense entity)
        {
            CreateConnection();
            var com = new SqlCommand("DeleteExpenseById", Connection) {CommandType = CommandType.StoredProcedure};

            com.Parameters.AddWithValue("@Id", entity.Id);

            Connection.Open();
            var result = com.ExecuteNonQuery();
            Connection.Close();
            return result >= 1;
        }
        

        public List<Expense> GetAll()
        {
            CreateConnection();
            var expensesList = new List<Expense>();

            var com = new SqlCommand("SelectAllExpenses", Connection) {CommandType = CommandType.StoredProcedure};
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            Connection.Open();
            dataAdapter.Fill(dataTable);
            Connection.Close();

            foreach (DataRow dr in dataTable.Rows)
            {
                expensesList.Add(
                    new Expense()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Amount = (float) Convert.ToDouble(dr["Amount"]),
                        CreatedTime = Convert.ToDateTime(dr["CreatedTime"]),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        UserId = Convert.ToInt32(dr["UserId"]),
                        Definition = Convert.ToString(dr["Definition"])
                    }
                    );
            }

            return expensesList;
        }

        public Expense GetById(int? id)
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
            list = (list.OrderByDescending(x => x.CreatedTime).ToList());
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
        public List<Expense> GetAllExpensesOfUser(int userId)
        {
            var list = GetAll();
            var listToReturn = list.Where(item => item.UserId == userId).ToList();
            listToReturn = (listToReturn.OrderByDescending(x => x.CreatedTime).ToList());
            return listToReturn;
        }

        public bool Insert(Expense entity)
        {
            CreateConnection();
            var command = new SqlCommand("InsertExpense", Connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Amount", entity.Amount);
            command.Parameters.AddWithValue("@CreatedTime", entity.CreatedTime);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Definition", entity.Definition);
            command.Parameters.AddWithValue("@CategoryId", entity.CategoryId);

            Connection.Open();
            var i = command.ExecuteNonQuery();
            Connection.Close();

            return i >= 1;
        }

        public bool Update(Expense entity)
        {
            CreateConnection();
            var command = new SqlCommand("UpdateExpense", Connection) {CommandType = CommandType.StoredProcedure};

            AddCommandParameters(command, entity);

            Connection.Open();
            var i = command.ExecuteNonQuery();
            Connection.Close();

            return i >= 1;
        }

        private void AddCommandParameters(SqlCommand command, Expense entity)
        {
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Amount", entity.Amount);
            command.Parameters.AddWithValue("@CreatedTime", entity.CreatedTime);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Definition", entity.Definition);
            command.Parameters.AddWithValue("@CategoryId", entity.CategoryId);
        }
    }
}
