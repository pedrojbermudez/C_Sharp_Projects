using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using FamilyAccounting.Utils;

namespace FamilyAccounting.Database.Source
{
    class SourceDb
    {
        DatabaseManager conn;

        public SourceDb()
        {
            conn = new DatabaseManager("127.0.0.1", "root", "toor", "family_accounting");
        }

        public void NewSource(string name, string total)
        {
            this.conn.SQLSentence("insert into " + Constants.Tables.money_source + " (name, total) values (\"" + name + "\", " + total + ");");
        }

        public void EditSource(int id, string name, string total)
        {
            this.conn.SQLSentence("update " + Constants.Tables.money_source + " set name=\"" + name + "\", total=" + total + " where id=" + id + ";");
        }

        public void DeleteSource(int id)
        {
            this.conn.SQLSentence("delete from " + Constants.Tables.money_source + " where id=" + id + ";");
        }

        public Dictionary<int, List<string>> GetSources(int currentElement, int totalElement)
        {
            Dictionary<int, List<string>> result = null;
            string sqlSentence = "select id, name, total from " + Constants.Tables.money_source + " limit " + currentElement + ", " + totalElement;
            using (MySqlCommand cmd = new MySqlCommand(sqlSentence, this.conn.GetConnection()))
            {
                MySqlDataReader reader = null;
                try
                {
                    reader = cmd.ExecuteReader();
                    result = new Dictionary<int, List<string>>();
                    while (reader.Read())
                    {
                        List<string> tmp = new List<string>();
                        tmp.Add(reader.GetString("name"));
                        tmp.Add(reader.GetFloat("total").ToString());
                        result.Add(int.Parse(reader[0].ToString()), tmp);
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

        public Dictionary<int, List<string>> GetSources()
        {
            return this.conn.SQLGetSentence("select id, name, total from " + Constants.Tables.money_source + ";", 3);
        }

        public Dictionary<int, List<string>> GetSource(int id)
        {
            return this.conn.SQLGetSentence("select id, name, total from " + Constants.Tables.money_source + " where id=" + id + ";", 3);
        }

        public int TotalSources()
        {
            return this.conn.Count(Constants.Tables.money_source.ToString());
        }
    }
}
