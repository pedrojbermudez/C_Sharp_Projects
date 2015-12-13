using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAccounting.Database
{
    class MovementDb
    {
        private DatabaseManager conn;

        public MovementDb()
        {
            conn = new DatabaseManager();
        }

        public void NewMovement(int sourceId, int categoryId, string name, float value)
        {
            float income, outgoing;
            if (value > 0)
            {
                income = value;
                outgoing = 0;
                UpdateTotal(sourceId, income);
            }
            else
            {
                income = 0;
                outgoing = value;
                UpdateTotal(sourceId, -outgoing);
            }
            string date = DateTimeOffset.Now.ToString("u").Substring(0, 10);

            conn.SQLSentence("insert into " + Constants.Tables.movement + " (source_id, category_id, name, date, income, outgoing) values (" + sourceId + ", " + categoryId + ", \"" + name + "\", \"" + date + "\", " + income + ", " + outgoing + ")");
        }

        public void EditMovement(int movementId, int sourceId, int categoryId, string name, float value, float oldValue)
        {
            float income, outgoing;
            if (value > 0)
            {
                income = value;
                outgoing = 0;
                UpdateTotal(sourceId, value);
            }
            else
            {
                income = 0;
                outgoing = value;
                UpdateTotal(sourceId, -value);
            }
            if (oldValue > 0)
            {
                UpdateTotal(sourceId, -oldValue);
            }
            else
            {
                UpdateTotal(sourceId, oldValue);
            }
            string date = DateTimeOffset.Now.ToString("u").Substring(0, 10);
            conn.SQLSentence("update " + Constants.Tables.movement + " set source_id=" + sourceId + ", category_id=" + categoryId + ", name=\"" + name + "\", date=\"" + date + "\", income=" + income.ToString("0.00") + ", outgoing=" + outgoing.ToString("0.00") + " where id=" + movementId + ";");

        }

        public void DeleteMovement(int movementId, int sourceId, float value)
        {
            conn.SQLSentence("delete from " + Constants.Tables.movement + " where id=" + movementId);
            UpdateTotal(sourceId, value > 0 ? -value : value);
        }

        private void UpdateTotal(int id, float total)
        {
            conn.SQLSentence("update " + Constants.Tables.money_source + " set total=total+" + total.ToString("0.00") + " where id=" + id);
        }

        public List<string>[] GetMovement(int id)
        {
            return conn.SQLGetSentence("select * from " + Constants.Tables.movement + " where id="+id+";");
        }

        public List<string>[] GetMovements()
        {
            return conn.SQLGetSentence("select * from " + Constants.Tables.movement + ";");
        }

        public List<string>[] GetMovements(int currentElement, int totalElement)
        {
            return conn.SQLGetSentence("select * from " + Constants.Tables.movement + " limit " + currentElement + ", " + totalElement + ";");
        }
    }
}
