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
        public  void CategoryUpdate(List<Category> categories)
        {
            foreach(var category in categories)
            {
                Insert(category);
            }
        }
        public Category GetById(int id)
        {
            return base.GetById(id, GetAll());
        }
        public Category GetByString(string str)
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
        public List<Category> GetAll()
        {
            CreateConnection();
            var categoryList = new List<Category>();

            var com = new SqlCommand("CategorySelect", connection) {CommandType = CommandType.StoredProcedure};
            var dataAdapter = new SqlDataAdapter(com);
            var dataTable = new DataTable();

            connection.Open();
            dataAdapter.Fill(dataTable);
            connection.Close();

            foreach (DataRow dr in dataTable.Rows)
            {
                categoryList.Add(
                    new Category()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        CategoryName = Convert.ToString(dr["category_name"]),
                        Aliases = Convert.ToString(dr["aliases"]),
                        IsBaseExpense = Convert.ToBoolean(dr["IsBaseExpense"])
                    }
                    );
            }

            return categoryList;
        }

        public bool Insert(Category entity)
        {
            CreateConnection();
            var command = new SqlCommand("CategoryInsert", connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@categoryName", entity.CategoryName);
            command.Parameters.AddWithValue("@aliases", entity.Aliases);
            command.Parameters.AddWithValue("@IsBaseExpense", entity.IsBaseExpense);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            return i >= 1;
        }

        public bool Update(Category entity)
        {
            CreateConnection();
            var command = new SqlCommand("CategoryUpdate", connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@categoryName", entity.CategoryName);
            command.Parameters.AddWithValue("@aliases", entity.Aliases);
            command.Parameters.AddWithValue("@IsBaseExpense", entity.IsBaseExpense);

            connection.Open();
            var i = command.ExecuteNonQuery();
            connection.Close();

            return i >= 1;
        }
        public bool Delete(Category entity)
        {
            CreateConnection();
            var com = new SqlCommand("CategoryDelete", connection) {CommandType = CommandType.StoredProcedure};

            com.Parameters.AddWithValue("@Id", entity.Id);

            connection.Open();
            var result = com.ExecuteNonQuery();
            connection.Close();

            return result >= 1;
        }
    }
}
