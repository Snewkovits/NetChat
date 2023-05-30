using MySqlConnector;

namespace NetChat
{
    internal class LoginPanel
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataReader reader;

        public User Login()
        {
            conn = new MySqlConnection(MySQL.ConnectionString);

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
                Console.CursorVisible = true;
                username = Console.ReadLine();
                Console.CursorVisible = false;
                Console.SetCursorPosition(entryLength, 1);

                Console.CursorVisible = true;
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
                            Environment.Exit(0);
                            break;
                        default:
                            password += key;
                            Console.Write('*');
                            break;
                    }
                }
                Console.CursorVisible = false;
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
                catch
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

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Successfully logged in!");
            Console.ForegroundColor = ConsoleColor.Gray;
            Thread.Sleep(1000);

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
