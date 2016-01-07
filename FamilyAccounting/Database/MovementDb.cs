using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyAccounting.Utils;
using MySql.Data.MySqlClient;

namespace FamilyAccounting.Database.Movement
{
    class MovementDb
    {
        private DatabaseManager conn;

        internal MovementDb()
        {
            conn = new DatabaseManager();
        }

        internal void NewMovement(int sourceId, int categoryId, string name, float value, string date)
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
                outgoing = -value;
                UpdateTotal(sourceId, -outgoing);
            }
            conn.SQLSentence("insert into " + Constants.Tables.movement + " (source_id, category_id, name, movement_date, income, outgoing) values (" + sourceId + ", " + categoryId + ", \"" + name + "\", \"" + date + "\", " + income.ToString().Replace(',', '.') + ", " + outgoing.ToString().Replace(',', '.') + ")");
        }

        internal void EditMovement(int movementId, int sourceId, int categoryId, string name, float value, float oldValue)
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
                outgoing = -value;
                UpdateTotal(sourceId, value);
            }
            if (oldValue > 0)
            {
                UpdateTotal(sourceId, -oldValue);
            }
            else
            {
                UpdateTotal(sourceId, -oldValue);
            }
            string date = DateTimeOffset.Now.ToString("u").Substring(0, 10);
            conn.SQLSentence("update " + Constants.Tables.movement + " set source_id=" + sourceId + ", category_id=" + categoryId + ", name=\"" + name + "\", movement_date=\"" + date + "\", income=" + income.ToString().Replace(',', '.') + ", outgoing=" + outgoing.ToString().Replace(',', '.') + " where id=" + movementId + ";");

        }

        internal void DeleteMovement(int movementId, int sourceId, float value)
        {
            conn.SQLSentence("delete from " + Constants.Tables.movement + " where id=" + movementId);
            UpdateTotal(sourceId, value > 0 ? -value : -value);
        }

        private void UpdateTotal(int id, float total)
        {
            conn.SQLSentence("update " + Constants.Tables.money_source + " set total=total+" + total.ToString().Replace(',', '.') + " where id=" + id);
        }

        internal Dictionary<int, List<string>> GetMovement(int id)
        {
            return conn.SQLGetSentence("select * from " + Constants.Tables.movement + " where id=" + id + ";", 1);
        }

        internal Dictionary<int, List<string>> GetMovements()
        {
            Dictionary<int, List<string>> result = null;
            string sqlMovement = Constants.Tables.movement + ".id, " + Constants.Tables.movement + ".name, " + Constants.Tables.movement + ".income, " + Constants.Tables.movement + ".outgoing, " + Constants.Tables.movement + ".movement_date, ";
            string sqlSource = Constants.Tables.movement + ".source_id, " + Constants.Tables.money_source + ".name as source_name, ";
            string sqlCategory = Constants.Tables.movement + ".category_id, " + Constants.Tables.category + ".name as category_name";
            string sqlSentence = "select " + sqlMovement + sqlSource + sqlCategory + " from " + Constants.Tables.movement + " inner join "
                + Constants.Tables.money_source + ", " + Constants.Tables.category + " on " + Constants.Tables.category + ".id=" + Constants.Tables.movement
                + ".category_id and " + Constants.Tables.money_source + ".id=" + Constants.Tables.movement + ".source_id order by movement_date desc;";
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
                        ///         name[0] 
                        ///         movement_date[1] 
                        ///         source_name[2]
                        ///         category_name[3]
                        ///         income[4]
                        ///         outgoing[5]
                        ///         source_id[6]
                        ///         category_id[7]
                        tmp.Add(reader.GetString("name"));
                        tmp.Add(reader.GetMySqlDateTime("movement_date").ToString());
                        tmp.Add(reader.GetString("source_name"));
                        tmp.Add(reader.GetString("category_name"));
                        tmp.Add(reader.GetFloat("income").ToString());
                        tmp.Add(reader.GetFloat("outgoing").ToString());
                        tmp.Add(reader.GetInt32("source_id").ToString());
                        tmp.Add(reader.GetInt32("category_id").ToString());
                        result.Add(reader.GetInt32("id"), tmp);
                    }
                }
                catch (MySqlException e)
                {
                    Console.Write("Getting error: " + e.Message);
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

        /// <summary>
        /// Get movements with limit.
        /// This return: 
        ///     id 
        ///     {
        ///         name[0] 
        ///         movement_date[1] 
        ///         source_name[2]
        ///         category_name[3]
        ///         income[4]
        ///         outgoing[5]
        ///         source_id[6]
        ///         category_id[7]
        ///     }
        /// </summary>
        /// <param name="currentElement"></param>
        /// <param name="totalElement"></param>
        /// <returns></returns>
        internal Dictionary<int, List<string>> GetMovements(int currentElement, int totalElement)
        {
            Dictionary<int, List<string>> result = null;
            string sqlMovement = Constants.Tables.movement + ".id, " + Constants.Tables.movement + ".name, " + Constants.Tables.movement + ".income, " + Constants.Tables.movement + ".outgoing, " + Constants.Tables.movement + ".movement_date, ";
            string sqlSource = Constants.Tables.movement + ".source_id, " + Constants.Tables.money_source 
                + ".name as source_name, ";
            string sqlCategory = Constants.Tables.movement + ".category_id, " + Constants.Tables.category 
                + ".name as category_name";
            string sqlSentence = "select " + sqlMovement + sqlSource + sqlCategory + " from " 
                + Constants.Tables.movement + " inner join " + Constants.Tables.money_source + " on  " 
                + Constants.Tables.movement + ".source_id=" 
                + Constants.Tables.money_source + ".id inner join "+ Constants.Tables.category + " on " 
                + Constants.Tables.movement + ".category_id=" + Constants.Tables.category
                + ".id where "+Constants.Tables.category+".id="+Constants.Tables.movement+".category_id and "
                + Constants.Tables.money_source     + ".id=" + Constants.Tables.movement 
                + ".source_id order by movement_date desc limit " + currentElement + ", " + totalElement + ";";
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
                        ///         name[0] 
                        ///         movement_date[1] 
                        ///         source_name[2]
                        ///         category_name[3]
                        ///         income[4]
                        ///         outgoing[5]
                        ///         source_id[6]
                        ///         category_id[7]
                        tmp.Add(reader.GetString("name"));
                        tmp.Add(reader.GetMySqlDateTime("movement_date").ToString());
                        tmp.Add(reader.GetString("source_name"));
                        tmp.Add(reader.GetString("category_name"));
                        tmp.Add(reader.GetFloat("income").ToString());
                        tmp.Add(reader.GetFloat("outgoing").ToString());
                        tmp.Add(reader.GetInt32("source_id").ToString());
                        tmp.Add(reader.GetInt32("category_id").ToString());
                        result.Add(reader.GetInt32("id"), tmp);
                    }
                }
                catch (MySqlException e)
                {
                    Console.Write("Getting Error " +e.Message);
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

        internal int TotalMovements()
        {
            return conn.Count(Constants.Tables.movement.ToString());
        }
    }
}
