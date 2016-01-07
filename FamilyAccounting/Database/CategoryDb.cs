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
        DatabaseManager conn;

        public CategoryDb()
        {
            conn = new DatabaseManager("127.0.0.1", "root", "toor", "family_accounting");
        }

        public void NewCategory(string name)
        {
            this.conn.SQLSentence("insert into " + Constants.Tables.category + " (name) values (\"" + name + "\");");
        }

        public void EditCategory(int id, string name)
        {
            this.conn.SQLSentence("update " + Constants.Tables.category + " set name=\"" + name + "\" where id=" + id + ";");
        }

        public void DeleteCategory(int id)
        {
            // Using XAMPP for Windows by default use MariaDB. MariaDB doesn't support SET DEFAULT on delete so you must update when a category is deleted.
            this.conn.SQLSentence("update " + Constants.Tables.movement + " set category_id=1 where category_id=" + id);
            this.conn.SQLSentence("delete from " + Constants.Tables.category + " where id=" + id + ";");
        }

        public Dictionary<int, string> GetCategories(int currentElement, int totalElement)
        {
            Dictionary<int, string> result = null;
            string sqlSentence = "select id, name from " + Constants.Tables.category + " where id > 1 limit " + currentElement + ", " + totalElement;
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

        public Dictionary<int, string> GetCategories()
        {
            Dictionary<int, string> result = null;
            string sqlSentence = "select id, name from " + Constants.Tables.category;
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

        public int TotalCategories()
        {
            return this.conn.Count(Constants.Tables.category.ToString());
        }
    }
}
