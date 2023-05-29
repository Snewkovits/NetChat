using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace NetChat
{
    internal class LoginPanel
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlConnectionStringBuilder connStringBuilder;
        MySqlDataReader reader;

        public User Login()
        {
            connStringBuilder = new MySqlConnectionStringBuilder() // messages (id, from, to, message, when)
            {                                                      // users (id, username, password)
                Server = "localhost",
                Port = 3306,
                UserID = "root",
                Password = "root",
                Database = "onlinechat"
            };

            conn = new MySqlConnection(connStringBuilder.ConnectionString);

            bool isLogged = false;
            bool enterPressed = false;
            int entryLength = "Username: ".Length;
            string username = string.Empty;
            string password = string.Empty;

            User user = new User()
            {
                Username = "N/A",
                Password = "N/A"
            };

            Console.Clear();
            while (!isLogged)
            {
                username = string.Empty;
                password = string.Empty;

                Console.Clear();
                enterPressed = false;
                Console.SetCursorPosition(0, 0);
                Console.Write("Username:\nPassword:");
                Console.SetCursorPosition(entryLength, 0);
                username = Console.ReadLine();
                Console.SetCursorPosition(entryLength, 1);

                while (!enterPressed)
                {
                    char key = Console.ReadKey(true).KeyChar;
                    switch (key)
                    {
                        case (char)13:
                            enterPressed = !enterPressed;
                            break;
                        case (char)8:
                            if (password.Length > 0)
                            {
                                Console.SetCursorPosition(entryLength + password.Length - 1, 1);
                                Console.Write(' ');
                                Console.SetCursorPosition(entryLength + password.Length - 1, 1);
                                password = password.Substring(0, password.Length - 1);
                            }
                            break;
                        case (char)27:
                            Program.oc.isOnline = false;
                            Environment.Exit(0);
                            break;
                        default:
                            password += key;
                            Console.Write('*');
                            break;
                    }
                }
                user = new User()
                {
                    Username = username,
                    Password = password
                };

                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT id, password FROM users WHERE username = @username";
                cmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = user.Username;

                try
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["password"].ToString() == user.Password)
                        {
                            isLogged = !isLogged;
                            user.UserID = int.Parse(reader["id"].ToString());
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("\nIncorrect password or username!");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.ReadKey(true);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"\nIncorrect password or username!\nERRMSQL01");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.ReadKey(true);
                }

                conn.Close();
            }

            if (user.Username == "N/A" || user.Password == "N/A")
                Environment.Exit(0);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nYou are logged in!");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ReadKey(true);

            return user;
        }
    }

    struct User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
