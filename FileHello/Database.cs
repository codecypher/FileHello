using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHello
{
    /// <summary>
    /// Database calls using SQLite
    /// </summary>
    /// SQLite Tutorial
    /// http://www.sqlitetutorial.net/
    /// https://www.techonthenet.com/sqlite/index.php
    class Database
    {
        // Use this Random object to choose random numbers to write to file.
        static Random random = new Random();

        // Connection string for SQLite database
        static string connectionString = "Data Source=sample.sqlite;Version=3";

        // Write a record to SQLite database using random data.
        // Added System.Data.SQLite package to project using NuGet.
        public static int WriteData()
        {
            int numRowsInserted = 0;

            // Get current time
            string timeString = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", DateTime.Now);

            // Get random integer between 0 and 100
            int randomInteger = random.Next(100);

            // Get random double between 0.0 and 100.0
            double randomDouble = random.NextDouble() * 100;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    using (SQLiteCommand command = new SQLiteCommand(conn))
                    {
                        string sql = "INSERT into Log (timestamp, temp1, temp2) " +
                            "VALUES ('{0}', {1:d3}, {2:f2});";
                        sql = String.Format(sql, timeString, randomInteger, randomDouble);
                        command.CommandText = sql;
                        numRowsInserted = command.ExecuteNonQuery();
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("An unexpected error occured in {0}.{1}: {2}",
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message);
                throw new ApplicationException(message);
            }

            return numRowsInserted;
        }

        /// Write a record to database.
        /// Sample: 2017-03-28 12:32:55.342, 019, 70.02
        /// <param name="data">CSV to insert in database</param>
        public static int WriteData(string data)
        {
            int numRowsInserted = 0;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    using (SQLiteCommand command = new SQLiteCommand(conn))
                    {
                        string sql = "INSERT into Log (timestamp, temp1, temp2) " +
                            "VALUES ('{0}', {1:d3}, {2:f2});";
                        string[] row = data.Split(',');
                        sql = String.Format(sql, row[0], row[1], row[2]);
                        command.CommandText = sql;
                        numRowsInserted = command.ExecuteNonQuery();
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("An unexpected error occured in {0}.{1}: {2}",
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message);
                throw new ApplicationException(message);
            }

            return numRowsInserted;
        }

        /// Find record that is closest to given datetime.
        /// Sample: 2018-03-28 12:32:55.342
        /// <param name="input">datetime to find as string</param>
        public static string FindByDateTime(string input)
        {
            string data = null;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    using (SQLiteCommand command = new SQLiteCommand(conn))
                    {
                        string sql = "SELECT * FROM Log WHERE strftime('%Y-%m-%d %H:%M:%f', timestamp) " +
                            "<= strftime('%Y-%m-%d %H:%M:%f','{0}') " +
                            "ORDER BY strftime('%Y-%m-%d %H:%M:%f', timestamp) DESC LIMIT 1;";
                        sql = String.Format(sql, input);

                        // Set sql for command and execute
                        command.CommandText = sql;
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // Query only returns one record that is closest match
                            if (reader.Read())
                            {
                                data = reader["timestamp"].ToString();
                                data += " " + reader["temp1"].ToString();
                                data += " " + reader["temp2"].ToString();
                            }

                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("An unexpected error occured in {0}.{1}: {2}",
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message);
                throw new ApplicationException(message);
            }

            return data;
        }

        // Create SQLite database
        public static void Create()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    //SQLiteConnection.CreateFile("sample.sqlite");

                    using (SQLiteCommand command = new SQLiteCommand(conn))
                    {
                        string sql = "CREATE TABLE Log (timestamp text NOT NULL, temp1 int, temp2 real);";
                        command.CommandText = sql;
                        int status = command.ExecuteNonQuery();

                        sql = "CREATE UNIQUE INDEX idx_timestamp on Log (timestamp);";
                        command.CommandText = sql;
                        status = command.ExecuteNonQuery();
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("An unexpected error occured in {0}.{1}: {2}",
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message);
                throw new ApplicationException(message);
            }
        }

        // Clear SQLite database
        public static int Clear()
        {
            int numRowsDeleted = 0;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand command = new SQLiteCommand(conn))
                    {
                        string sql = "DROP TABLE Log;";
                        command.CommandText = sql;
                        numRowsDeleted = command.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("An unexpected error occured in {0}.{1}: {2}",
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message);
                throw new ApplicationException(message);
            }

            return numRowsDeleted;
        }
    }
}
