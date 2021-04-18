using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository.Abstractions;

namespace sucks_bucks_bot.Repository
{
    public class IncomeRepository: AbstractRepo<Income>, IRepository<Income>
    {
        public bool Delete(Income entity)
        {
            CreateConnection();
            var com = new SqlCommand("DeleteIncomeById", connection) {CommandType = CommandType.StoredProcedure};

            com.Parameters.AddWithValue("@Id", entity.Id);

            connection.Open();
            var result = com.ExecuteNonQuery();
            connection.Close();
            return result >= 1;
        }
        

        public List<Income> GetAll()
        {
            CreateConnection();
            var expensesList = new List<Income>();

            var com = new SqlCommand("SelectAllIncomes", connection) {CommandType = CommandType.StoredProcedure};
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            connection.Open();
            dataAdapter.Fill(dataTable);
            connection.Close();

            foreach (DataRow dr in dataTable.Rows)
            {
                expensesList.Add(
                    new Income()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Amount = (float) Convert.ToDouble(dr["Amount"]),
                        CreatedTime = Convert.ToDateTime(dr["CreatedTime"]),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        UserId = Convert.ToInt32(dr["UserId"]),
                        Definition = Convert.ToString(dr["Definition"]),
                        ExpiresAt = Convert.ToDateTime(dr["ExpiresAt"])
                    }
                    );
            }

            return expensesList;
        }

        public Income GetById(int? id)
        {
            return base.GetById(id, GetAll());
        }
        public List<Income> GetByCategoryId(int categoryId, List<Income> list)
        {
            var listToReturn = new List<Income>();
            foreach (var item in list)
            {
                if (item.CategoryId == categoryId)
                {
                    listToReturn.Add(item);
                }
            }
            return listToReturn;
        }

        public List<Income> GetFirstByTime(int number, List<Income> list)
        {
            var listToReturn = new List<Income>();
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
        public List<Income> GetAllExpensesOfUser(int userId)
        {
            var list = GetAll();
            var listToReturn = list.Where(item => item.UserId == userId).ToList();
            listToReturn = (listToReturn.OrderByDescending(x => x.CreatedTime).ToList());
            return listToReturn;
        }

        public bool Insert(Income entity)
        {
            CreateConnection();
            var command = new SqlCommand("InsertIncome", connection) {CommandType = CommandType.StoredProcedure};

            AddCommandParameters(command, entity);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            return i >= 1;
        }

        public bool Update(Income entity)
        {
            CreateConnection();
            var command = new SqlCommand("UpdateIncome", connection) {CommandType = CommandType.StoredProcedure};

            AddCommandParameters(command, entity);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            return i >= 1;
        }

        private void AddCommandParameters(SqlCommand command, Income entity)
        {
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Amount", entity.Amount);
            command.Parameters.AddWithValue("@CreatedTime", entity.CreatedTime);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Definition", entity.Definition);
            command.Parameters.AddWithValue("@CategoryId", entity.CategoryId);
            command.Parameters.AddWithValue(@"ExpiresAt", entity.ExpiresAt);
        }
    }
}