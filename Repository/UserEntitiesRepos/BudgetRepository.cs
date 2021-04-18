using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository.Abstractions;

namespace sucks_bucks_bot.Repository
{
    public class BudgetRepository: AbstractRepo<Budget>, IGenericRepository<Budget>
    {
        public Budget GetById(int? id)
        {
            return base.GetById(id, GetAll());
        }

        public List<Budget> GetAll()
        {
            CreateConnection();
            var budgetList = new List<Budget>();

            var com = new SqlCommand("SelectAllBudgets", Connection) {CommandType = CommandType.StoredProcedure};
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            Connection.Open();
            dataAdapter.Fill(dataTable);
            Connection.Close();
   
            foreach (DataRow dr in dataTable.Rows)
            {
                budgetList.Add(
                    new Budget()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        MonthlyBudget = (float) Convert.ToDouble(dr["monthly_budget"]),
                        WeeklyBudget = (float) Convert.ToDouble(dr["weekly_budget"]),
                        DailyBudget = (float) Convert.ToDouble(dr["daily_budget"]),
                        Comment = Convert.ToString(dr["comment"]),
                        UserId = Convert.ToInt32(dr["User_Id"])
                    }
                    );
            }

            return budgetList;
        }

        public bool Update(Budget entity)
        { 
            CreateConnection();
            var command = new SqlCommand("UpdateBudget", Connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@monthBudget", entity.MonthlyBudget);
            command.Parameters.AddWithValue("@weeklybudget", entity.WeeklyBudget);
            command.Parameters.AddWithValue("@dailybudget", entity.DailyBudget);
            command.Parameters.AddWithValue("@comment", entity.Comment);
            command.Parameters.AddWithValue("@userId", entity.UserId);

            Connection.Open();
            var result = command.ExecuteNonQuery();
            Connection.Close();

            return result >= 1;
        }

        public bool Insert(Budget entity)
        {
            CreateConnection();
            var command = new SqlCommand("InsertBudget", Connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@monthBudget", entity.MonthlyBudget);
            command.Parameters.AddWithValue("@weeklybudget", entity.WeeklyBudget);
            command.Parameters.AddWithValue("@dailybudget", entity.DailyBudget);
            command.Parameters.AddWithValue("@comment", entity.Comment);
            command.Parameters.AddWithValue("@userId", entity.UserId);

            Connection.Open();
            var i = command.ExecuteNonQuery();
            Connection.Close();

            return i >= 1;
        }

        public bool Delete(Budget entity)
        {
            CreateConnection();
            var com = new SqlCommand("DeleteBudgetById", Connection) {CommandType = CommandType.StoredProcedure};

            com.Parameters.AddWithValue("@Id", entity.Id);

            Connection.Open();
            var result = com.ExecuteNonQuery();
            Connection.Close();
            return result >= 1;
        }
    }
}
