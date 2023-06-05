using MySqlConnector;

namespace NetChat
{
    internal class MySQL
    {
        public static bool isSetted = false;
        static string databaseFile = "database.snw";

        static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder()
        {
            Server = "0",
            Port = 0,
            UserID = "0",
            Password = "0",
            Database = "0"
        };

        public static string ConnectionString = builder.ConnectionString;
    
        public static void SetMySql(string server = null, int port = 0, string uid = null, string password = null, string database = null)
        {
            if (server == null)
            {
                if (File.Exists(databaseFile))
                {
                    string datas = string.Empty;
                    using (StreamReader sr = new StreamReader(databaseFile))
                    {
                        datas = sr.ReadLine();
                    }
                    if (int.Parse(datas.Split(";")[0]) == 0)
                    {
                        MySqlMenu();
                        return;
                    }
                    else
                    {
                        List<string> result = datas.Split(";").ToList();
                        ConnectionString = new MySqlConnectionStringBuilder()
                        {
                            Server = result[1],
                            Port = uint.Parse(result[2]),
                            UserID = result[3],
                            Password = result[4],
                            Database = result[5]
                        }.ConnectionString;
                    }
                }
                else
                {
                    Console.Write("Connection file generated.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    using (StreamWriter sr = new StreamWriter(databaseFile))
                    {
                        sr.Write("0;0;0;0;0;0");
                    }
                    SetMySql();
                }
            }
        }

        public static void MySqlMenu()
        {

            List<string> menuItems = new List<string>()
            {
                "Server: ",
                "Port: ",
                "UserID: ",
                "Password: ",
                "Database: "
            };

            List<string> result = new List<string>();
            bool selected = false;
            bool isSave = true;
            
            while (!Test())
            {
                Console.Clear();
                if (selected)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("The connection was not established!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Thread.Sleep(2000);
                    Console.Clear();
                }
                selected = true;
                result = new List<string>();
                foreach(string item in menuItems)
                    Console.WriteLine(item);
                Console.CursorVisible = true;
                for (int i = 0; i < menuItems.Count; i++)
                {
                    Console.SetCursorPosition(menuItems[i].Length, i);
                    result.Add(Console.ReadLine());
                }
                Console.CursorVisible = false;
                Console.Clear();
                Console.Write("Attempt to connect to the database.");
                try
                {
                    ConnectionString = new MySqlConnectionStringBuilder()
                    {
                        Server = result[0],
                        Port = uint.Parse(result[1]),
                        UserID = result[2],
                        Password = result[3],
                        Database = result[4]
                    }.ConnectionString;
                }
                catch
                {
                    ConnectionString = new MySqlConnectionStringBuilder()
                    {
                        Server = "0",
                        Port = 0,
                        UserID = "0",
                        Password = "0",
                        Database = "0"
                    }.ConnectionString;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("The port is not in the correct datatype!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Thread.Sleep(2000);
                }
            }
            selected = false;


            while (!selected)
            {
                Console.Clear();
                Console.Write("Do you want to save this connection?\n");
                if (isSave)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Yes");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("\tNo");
                }
                else
                {
                    Console.Write("Yes");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\tNo");
                    Console.ForegroundColor = ConsoleColor.Gray;

                }
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.RightArrow:
                        isSave = !isSave;
                        break;
                    case ConsoleKey.LeftArrow:
                        isSave = !isSave;
                        break;
                    case ConsoleKey.Enter:
                        selected = !selected;
                        break;
                }

            }

            Console.Clear();

            if (isSave)
            {
                using (StreamWriter sr = new StreamWriter(databaseFile))
                {
                    sr.Write($"1;{string.Join(";", result)}");
                }
            }
        }

        static bool Test()
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            try { conn.Open(); conn.Close(); return true; }
            catch { return false; }
        }
    }
}

/*
      

CREATE DATABASE IF NOT EXISTS `onlinechat` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `onlinechat`;     
     
CREATE TABLE IF NOT EXISTS `messages` (
    `id` int NOT NULL AUTO_INCREMENT,
    `sender` int DEFAULT NULL,
    `receiver` int DEFAULT NULL,
    `message` varchar(250) COLLATE utf8mb4_hungarian_ci DEFAULT NULL,
    `sended` datetime DEFAULT NULL,
    PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

CREATE TABLE IF NOT EXISTS `users` (
    `id` int DEFAULT NULL,
    `username` varchar(50) COLLATE utf8mb4_hungarian_ci DEFAULT NULL,
    `password` varchar(50) COLLATE utf8mb4_hungarian_ci DEFAULT NULL,
    `last_available` datetime DEFAULT NULL,
    `isonline` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;


*/