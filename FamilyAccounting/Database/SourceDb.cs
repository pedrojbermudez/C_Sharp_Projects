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

        public void NewSource(string name, float total)
        {
            this.conn.SQLSentence("insert into " + Constants.Tables.money_source + " (name, total) values (\"" + name + "\", " + total.ToString("0.00") + ");");
        }

        public void EditSource(int id, string name, float total)
        {
            this.conn.SQLSentence("update " + Constants.Tables.money_source + " set name=\"" + name + "\", total=" + total.ToString("0.00") + " where id=" + id + ";");
        }

        public void DeleteSource(int id)
        {

            this.conn.SQLSentence("delete from " + Constants.Tables.money_source + " where id=" + id + ";");
        }

        public Dictionary<int, List<string>> GetSources(int currentElement, int totalElement)
        {
            return this.conn.SQLGetSentence("select id, name, total from " + Constants.Tables.money_source + " limit " + currentElement + ", " + totalElement + ";");
        }

        public Dictionary<int, List<string>> GetSources()
        {
            return this.conn.SQLGetSentence("select id, name, total from " + Constants.Tables.money_source + ";");
        }

        public Dictionary<int, List<string>> GetSource(int id)
        {
            return this.conn.SQLGetSentence("select id, name, total from " + Constants.Tables.money_source + " where id=" + id + ";");
        }

        public int TotalSources()
        {
            return this.conn.Count(Constants.Tables.money_source.ToString());
        }
    }
}
