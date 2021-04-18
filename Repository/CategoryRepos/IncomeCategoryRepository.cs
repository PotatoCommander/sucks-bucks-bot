using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using sucks_bucks_bot.Model;
using sucks_bucks_bot.Repository.Abstractions;

namespace sucks_bucks_bot.Repository.CategoryRepos
{
    public class IncomeCategoryRepository: AbstractRepo<IncomeCategory>, IGenericRepository<IncomeCategory>
    {
        public  void CategoryUpdate(List<IncomeCategory> categories)
        {
            foreach(IncomeCategory category in categories)
            {
                Insert(category);
            }
        }
        public IncomeCategory GetById(int? id)
        {
            return base.GetById(id, GetAll());
        }
        public IncomeCategory GetByString(string str)
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
        public List<IncomeCategory> GetAll()
        {
            CreateConnection();
            var categoryList = new List<IncomeCategory>();

            var com = new SqlCommand("SelectAllIncomeCategories", Connection) {CommandType = CommandType.StoredProcedure};
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            Connection.Open();
            dataAdapter.Fill(dataTable);
            Connection.Close();

            foreach (DataRow dr in dataTable.Rows)
            {
                categoryList.Add(
                    new IncomeCategory
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        CategoryName = Convert.ToString(dr["CategoryName"]),
                        Aliases = Convert.ToString(dr["Aliases"]),
                        UserId = Convert.IsDBNull(dr["UserId"])? null :(Convert.ToInt32(dr["UserId"]))
                    }
                    );
            }

            return categoryList;
        }

        public bool Insert(IncomeCategory entity)
        {
            CreateConnection();
            var command = new SqlCommand("InsertIncomeCategory", Connection) {CommandType = CommandType.StoredProcedure};

            AddCommandParameters(command, entity);

            Connection.Open();
            var i = command.ExecuteNonQuery();
            Connection.Close();

            return i >= 1;
        }

        public bool Update(IncomeCategory entity)
        {
            CreateConnection();
            var command = new SqlCommand("UpdateIncomeCategories", Connection) {CommandType = CommandType.StoredProcedure};
            
            AddCommandParameters(command, entity);
            
            Connection.Open();
            var i = command.ExecuteNonQuery();
            Connection.Close();

            return i >= 1;
        }
        public bool Delete(IncomeCategory entity)
        {
            CreateConnection();
            var com = new SqlCommand("DeleteIncomeCategoryById", Connection) {CommandType = CommandType.StoredProcedure};

            com.Parameters.AddWithValue("@Id", entity.Id);

            Connection.Open();
            var result = com.ExecuteNonQuery();
            Connection.Close();

            return result >= 1;
        }

        private void CreateCommand(string nameOfCooma)
        {
            
        }

        private void AddCommandParameters(SqlCommand command, IncomeCategory entity)
        {
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@CategoryName", entity.CategoryName);
            command.Parameters.AddWithValue("@Aliases", entity.Aliases);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
        }
    }
}