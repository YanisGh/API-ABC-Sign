using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace APInancy
{
    public class DBconn
    {
        private static string connectionString = "server=localhost;user=root;password=;database=abcsignapi";

        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
