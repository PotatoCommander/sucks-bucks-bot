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
    class CategoryRepository : AbstractRepo<Category>, IRepository<Category>
    {
        public Entity GetById(int id)
        {
            return base.GetById(id, GetAll());
        }
        public List<Category> GetAll()
        {
            CreateConnection();
            var CategoryList = new List<Category>();

            var com = new SqlCommand("CategorySelect", connection);
            com.CommandType = CommandType.StoredProcedure;
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            connection.Open();
            dataAdapter.Fill(dataTable);
            connection.Close();

            foreach (DataRow dr in dataTable.Rows)
            {
                CategoryList.Add(
                    new Category()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        CategoryName = Convert.ToString(dr["category_name"]),
                        Aliases = Convert.ToString(dr["aliases"]),
                        isBaseExpense = Convert.ToBoolean(dr["isBaseExpense"])
                    }
                    );
            }

            return CategoryList;
        }

        public bool Insert(Category entity)
        {
            CreateConnection();
            var command = new SqlCommand("CategoryInsert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@categoryName", entity.CategoryName);
            command.Parameters.AddWithValue("@aliases", entity.Aliases);
            command.Parameters.AddWithValue("@isBaseExpense", entity.isBaseExpense);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            if (i >= 1)
            {
                return true;
            }
            return false;
        }

        public bool Update(Category entity)
        {
            CreateConnection();
            var command = new SqlCommand("CategoryUpdate", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@categoryName", entity.CategoryName);
            command.Parameters.AddWithValue("@aliases", entity.Aliases);
            command.Parameters.AddWithValue("@isBaseExpense", entity.isBaseExpense);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            if (i >= 1)
            {
                return true;
            }
            return false;
        }
        public bool Delete(Category entity)
        {
            CreateConnection();
            var com = new SqlCommand("CategoryDelete", connection);

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
