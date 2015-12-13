using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace FamilyAccounting.Database
{
    class DatabaseManager
    {
        private static MySqlConnection conn;
        private string dataConnection;

        /// <summary>
        /// Default user, password, database and server
        /// </summary>
        public DatabaseManager()
        {
            dataConnection = "server=127.0.0.1;uid=root;pwd=toor;"
                + "database=family_acounting;";
            conn = new MySqlConnection(dataConnection);
            CreateTables();
        }

        /// <summary>
        /// Can write the server
        /// </summary>
        /// <param name="host">Host name or IP</param>
        public DatabaseManager(string host)
        {
            dataConnection = "server=" + host + ";uid=root;pwd=toor;"
                + "database=family_acounting;";
            conn = new MySqlConnection(dataConnection);
            CreateTables();
        }

        /// <summary>
        /// Can write the server, user and password
        /// </summary>
        /// <param name="host">Host name or IP</param>
        /// <param name="user">Username</param>
        /// <param name="password">User's password</param>
        public DatabaseManager(string host, string user, string password)
        {
            dataConnection = "server=" + host + ";uid=" + user + ";pwd=" + password + ";"
                + "database=family_acounting;";
            conn = new MySqlConnection(dataConnection);
            CreateTables();
        }

        /// <summary>
        /// Can write the server, user, password and database.
        /// </summary>
        /// <param name="host">Host name or IP</param>
        /// <param name="user">Username</param>
        /// <param name="password">User's password</param>
        /// <param name="database">Database used</param>
        public DatabaseManager(string host, string user, string password, string database)
        {
            dataConnection = "SERVER=" + host + ";UID=" + user + ";PASSWORD=" + password + ";"
                + "DATABASE=" + database + ";";
            conn = new MySqlConnection(dataConnection);
            CreateTables();
        }

        /// <summary>
        /// Realize a connection to the database.
        /// </summary>
        /// <returns>Return true if it was possible to connect, false if it was not.</returns>
        private bool DBConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Close connection to the database.
        /// </summary>
        /// <returns>true it was possible to close, false otherwise</returns>
        private bool CloseConnection()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Console.Write(e.Message);
            }
            return true;
        }

        /// <summary>
        /// Create table sql sentece when the object is created.
        /// </summary>
        private void CreateTables()
        {
            if (this.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    // create table movement
                    "create table if not exists " + Constants.Tables.category.ToString() + " (id int PRIMARY KEY AUTOINCREMENT, name blob not null); "
                    //create table money_source
                    + "create table if not exists " + Constants.Tables.money_source.ToString() + " (id int PRIMARY KEY AUTOINCREMENT, name blob not null, total int not null); "
                    // create table movement
                    + "create table if not exists " + Constants.Tables.movement.ToString() + "(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, source_id int not null, category_id int not null, name mediumblob not null, movement_date date not null, income int, outgoing int, constraint source_id_movementsfk foreign key(source_id) references " + Constants.Tables.money_source.ToString() + "(id) on update cascade on delete cascade, constraint category_id_movementfk foreign key(category_id) references " + Constants.Tables.category.ToString() + "(id) on update cascade on delete cascade);", conn))
                {
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
        }

        /// <summary>
        /// Update, delete, insert
        /// </summary>
        /// <param name="sqlSentence">The sql sentence.</param>
        /// <returns></returns>
        public bool SQLSentence(string sqlSentence)
        {
            if (this.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(sqlSentence, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        this.CloseConnection();
                        return true;
                    }
                    catch (MySqlException e)
                    {
                        Console.Write(e.Message);
                        this.CloseConnection();
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Used by select sentences
        /// </summary>
        /// <param name="sqlSentence">The select sentence</param>
        /// <returns>A list with the query result</returns>
        public List<string>[] SQLGetSentence(string sqlSentence)
        {

            List<string>[] result = null;
            if (this.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(sqlSentence, conn))
                {
                    MySqlDataReader reader = null;
                    try
                    {
                        reader = cmd.ExecuteReader();
                        DataTable tbl = new DataTable();
                        tbl.Load(reader);
                        int totalRows = tbl.Rows.Count;
                        int totalFields = tbl.Columns.Count;
                        result = new List<string>[totalRows];
                        int currentRow = 0;
                        while (reader.Read())
                        {
                            result[currentRow] = new List<string>();
                            for (int i = 0; i < totalFields; i++)
                            {
                                result[currentRow].Add(reader[i].ToString());
                            }
                            currentRow++;
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
                        this.CloseConnection();
                    }

                }
            }
            return result;
        }


        /// <summary>
        /// Count how many id has the query.
        /// </summary>
        /// <param name="table"></param>
        /// <returns>the total count of id</returns>
        public int Count(string table)
        {
            if (this.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("select id from " + table + ";", conn))
                {
                    int count = int.Parse(cmd.ExecuteScalar().ToString());
                    this.CloseConnection();
                    return count;
                }
            }
            else
            {
                return -1;
            }
        }
    }
}