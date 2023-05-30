using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat
{
    class Test
    {
        MySqlConnection conn;

        public Test()
        {
            conn = new MySqlConnection(MySQL.ConnectionString);
        }

        public bool Database()
        {
            bool result = true;
            try { conn.Open(); }
            catch { result = false; }
            return result;
        }
    }
}
