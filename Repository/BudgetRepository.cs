using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sucks_bucks_bot.Model;

namespace sucks_bucks_bot.Repository
{
    class BudgetRepository: AbstractRepo<Budget>, IRepository<Budget>
    {
        public Entity GetById(int id)
        {
            return base.GetById(id, GetAll());
        }

        public List<Budget> GetAll()
        {
            CreateConnection();
            var BudgetList = new List<Budget>();

            var com = new SqlCommand("BudgetSelect", connection);
            com.CommandType = CommandType.StoredProcedure;
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            connection.Open();
            dataAdapter.Fill(dataTable);
            connection.Close();
   
            foreach (DataRow dr in dataTable.Rows)
            {
                BudgetList.Add(
                    new Budget()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        MonthlyBudget = Convert.ToInt32(dr["monthly_budget"]),
                        WeeklyBudget = Convert.ToInt32(dr["weekly_budget"]),
                        DailyBudget = Convert.ToInt32(dr["daily_budget"]),
                        Comment = Convert.ToString(dr["comment"]),
                        UserId = Convert.ToInt32(dr["User_Id"])
                    }
                    );
            }

            return BudgetList;
        }

        public bool Update(Budget entity)
        { 
            CreateConnection();
            var command = new SqlCommand("BudgetUpdate", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@monthBudget", entity.MonthlyBudget);
            command.Parameters.AddWithValue("@weeklybudget", entity.WeeklyBudget);
            command.Parameters.AddWithValue("@dailybudget", entity.DailyBudget);
            command.Parameters.AddWithValue("@comment", entity.Comment);
            command.Parameters.AddWithValue("@userId", entity.UserId);

            connection.Open();
            var result = command.ExecuteNonQuery();
            connection.Close();

            if (result >= 1)
            {
                return true;
            }
            return false;
        }

        public bool Insert(Budget entity)
        {
            CreateConnection();
            var command = new SqlCommand("BudgetInsert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@monthBudget", entity.MonthlyBudget);
            command.Parameters.AddWithValue("@weeklybudget", entity.WeeklyBudget);
            command.Parameters.AddWithValue("@dailybudget", entity.DailyBudget);
            command.Parameters.AddWithValue("@comment", entity.Comment);
            command.Parameters.AddWithValue("@userId", entity.UserId);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            if (i >= 1)
            {
                return true;
            }
            return false;
        }

        public bool Delete(Budget entity)
        {
            CreateConnection();
            var com = new SqlCommand("BudgetDelete", connection);

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
    }
}
