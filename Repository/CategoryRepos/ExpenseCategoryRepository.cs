using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository.Abstractions;

namespace sucks_bucks_bot.Repository.CategoryRepos
{
    public class ExpenseCategoryRepository : AbstractRepo<ExpenseCategory>, IGenericRepository<ExpenseCategory>
    {
        public  void CategoryUpdate(List<ExpenseCategory> categories)
        {
            foreach(var category in categories)
            {
                Insert(category);
            }
        }
        public ExpenseCategory GetById(int? id)
        {
            return base.GetById(id, GetAll());
        }
        public ExpenseCategory GetByString(string str)
        {
            var list = GetAll();
            foreach (var item in list)
            {
                if (item.Aliases.Contains(str))
                {
                    return item;
                }
            }
            return GetByString("OTHER");
        }
        public List<ExpenseCategory> GetAll()
        {
            CreateConnection();
            var categoryList = new List<ExpenseCategory>();

            var com = new SqlCommand("SelectAllExpenseCategories", Connection) {CommandType = CommandType.StoredProcedure};
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            Connection.Open();
            dataAdapter.Fill(dataTable);
            Connection.Close();

            foreach (DataRow dr in dataTable.Rows)
            {
                categoryList.Add(
                    new ExpenseCategory
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        CategoryName = Convert.ToString(dr["CategoryName"]),
                        Aliases = Convert.ToString(dr["Aliases"]),
                        IsBaseExpense = Convert.ToBoolean(dr["IsBaseExpense"]),
                        UserId = Convert.IsDBNull(dr["UserId"])? null :(Convert.ToInt32(dr["UserId"]))
                    }
                    );
            }

            return categoryList;
        }

        public bool Insert(ExpenseCategory entity)
        {
            CreateConnection();
            var command = new SqlCommand("InsertExpenseCategory", Connection) {CommandType = CommandType.StoredProcedure};

            AddCommandParameters(command, entity);

            Connection.Open();
            var i = command.ExecuteNonQuery();
            Connection.Close();

            return i >= 1;
        }

        public bool Update(ExpenseCategory entity)
        {
            CreateConnection();
            var command = new SqlCommand("UpdateExpenseCategories", Connection) {CommandType = CommandType.StoredProcedure};
            
            AddCommandParameters(command, entity);
            
            Connection.Open();
            var i = command.ExecuteNonQuery();
            Connection.Close();

            return i >= 1;
        }
        public bool Delete(ExpenseCategory entity)
        {
            CreateConnection();
            var com = new SqlCommand("DeleteExpenseCategoryById", Connection) {CommandType = CommandType.StoredProcedure};

            com.Parameters.AddWithValue("@Id", entity.Id);

            Connection.Open();
            var result = com.ExecuteNonQuery();
            Connection.Close();

            return result >= 1;
        }

        private void AddCommandParameters(SqlCommand command, ExpenseCategory entity)
        {
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@CategoryName", entity.CategoryName);
            command.Parameters.AddWithValue("@Aliases", entity.Aliases);
            command.Parameters.AddWithValue("@IsBaseExpense", entity.IsBaseExpense);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
        }
    }
}
