using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FamilyAccounting.Database
{
    class SourceDb
    {
        DatabaseManager conn;

        public SourceDb()
        {
            conn = new DatabaseManager();
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

        public List<string>[] GetSources(int currentElement, int totalElement)
        {
            return this.conn.SQLGetSentence("select * from " + Constants.Tables.money_source + " limit " + currentElement + ", " + totalElement + ";");
        }

        public List<string>[] GetSources()
        {
            return this.conn.SQLGetSentence("select * from " + Constants.Tables.money_source + ";");
        }

        public void GetSource(int id)
        {
            List<string>[] tmp = this.conn.SQLGetSentence("select * from " + Constants.Tables.money_source + " where id=" + id + ";");
        }
    }
}
