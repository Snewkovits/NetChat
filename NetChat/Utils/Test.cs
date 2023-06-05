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
        public MySqlConnection conn;

        public Test()
        {
            conn = new MySqlConnection(MySQL.ConnectionString);
        }

        public void Database()
        {
            try 
            { 
                conn.Open(); 
                conn.Close();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Connection established!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Thread.Sleep(1000);
                Console.Clear();
            }
            catch 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("The program can't connect to the database!\nCheck your internet connection or try to contact with a support!\n\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Press something to exit...");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }
    }
}
