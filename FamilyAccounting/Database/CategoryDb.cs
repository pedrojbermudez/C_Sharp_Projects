using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyAccounting.Utils;
using MySql.Data.MySqlClient;

namespace FamilyAccounting.Database.Category
{
    class CategoryDb
    {
        private DatabaseManager conn;

        public CategoryDb()
        {
            conn = new DatabaseManager();
        }

        public void NewCategory(string name)
        {
            this.conn.SQLSentence("insert into " + Constants.Tables.category + " (name, total) values (\"" + name + "\");");
        }

        public void EditCategory(int id, string name)
        {
            this.conn.SQLSentence("update " + Constants.Tables.category + " set name=\"" + name + "\" where id=" + id + ";");
        }

        public void DeleteCategory(int id)
        {

            this.conn.SQLSentence("delete from " + Constants.Tables.category + " where id=" + id + ";");
        }

        public Dictionary<int, string> GetCategories(int currentElement, int totalElement)
        {
            Dictionary<int, string> result = null;
            string sqlSentence = "select id, name from " + Constants.Tables.category + " limit " + currentElement + ", " + totalElement;
            using (MySqlCommand cmd = new MySqlCommand(sqlSentence, this.conn.GetConnection()))
            {
                MySqlDataReader reader = null;
                try
                {
                    reader = cmd.ExecuteReader();
                    result = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        result.Add(int.Parse(reader[0].ToString()), reader.GetString("name"));
                    }
                }
                catch (MySqlException e)
                {
                    Console.Write(e.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    this.conn.CloseConnection();
                }

            }
            return result;
        }

        public Dictionary<int, List<string>> GetCategories()
        {
            return this.conn.SQLGetSentence("select id, name from " + Constants.Tables.category + ";", 2);
        }

        public int TotalCategories()
        {
            return this.conn.Count(Constants.Tables.category.ToString());
        }
    }
}
