using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyAccounting.Utils;

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

        public Dictionary<int, List<string>> GetCategories(int currentElement, int totalElement)
        {
            return this.conn.SQLGetSentence("select id, name from " + Constants.Tables.category + " limit " + currentElement + ", " + totalElement + ";", 3);
        }

        public Dictionary<int, List<string>> GetCategories()
        {
            return this.conn.SQLGetSentence("select id, name from " + Constants.Tables.category + ";", 2);
        }
    }
}
