using MySqlConnector;
using System.ComponentModel;

namespace NetChat
{
    internal class MessagePanel
    {
        User user { get; set; }
        Partner partner { get; set; }

        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataReader reader;

        bool isIdle;
        bool onMessage;
        int messagesCount = 0;

        public MessagePanel(User user, Partner partner)
        {
            this.user = user;
            this.partner = partner;

            conn = new MySqlConnection(MySQL.ConnectionString);

            isIdle = true;
            onMessage = false;
        }

        public void WriteAllMessages()
        {
            Console.Title = $"{partner.Username} | Chat";
            Console.Clear();
            List<Message> messages = new List<Message>();

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM messages WHERE sender = @sender AND receiver = @receiver";
            cmd.Parameters.Add("@sender", MySqlDbType.Int32).Value = user.UserID;
            cmd.Parameters.Add("@receiver", MySqlDbType.Int32).Value = partner.UserID;

            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                messages.Add(new Message
                {
                    From = int.Parse(reader["sender"].ToString()),
                    To = int.Parse(reader["receiver"].ToString()),
                    Content = reader["message"].ToString(),
                    When = (DateTime)reader["sended"]
                });
            }
            reader.Close();
            conn.Close();


            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM messages WHERE sender = @sender AND receiver = @receiver";
            cmd.Parameters.Add("@sender", MySqlDbType.Int32).Value = partner.UserID;
            cmd.Parameters.Add("@receiver", MySqlDbType.Int32).Value = user.UserID;

            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                messages.Add(new Message
                {
                    From = int.Parse(reader["sender"].ToString()),
                    To = int.Parse(reader["receiver"].ToString()),
                    Content = reader["message"].ToString(),
                    When = (DateTime)reader["sended"]
                });
            }
            reader.Close();
            conn.Close();

            messages = messages.SortByTime();
            messagesCount = messages.Count();

            foreach (Message message in messages)
            {
                if (message.From == user.UserID)
                {
                    Console.Write($"\n\t\t{message.Content}\n");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"\t\tYou {message.When}\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.Write($"\n{message.Content}\n");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"{partner.Username} {message.When}\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        public void StartChat()
        {
            char pressedKey;
            string message = string.Empty;
            bool isGoing = true;

            Console.CursorVisible = true;
            Console.Write(": ");

            while (isGoing)
            {
                pressedKey = Console.ReadKey(true).KeyChar;

                switch (pressedKey)
                {
                    case (char)27:  // ESCAPE
                        isGoing = false;
                        onMessage = false;
                        WriteAllMessages();
                        Console.CursorVisible = false;
                        break;
                    case (char)8:   // BACKSPACE
                        if (message.Length > 0)
                        {
                            Console.SetCursorPosition(": ".Length + message.Length - 1, Console.CursorTop);
                            Console.Write(" ");
                            message = message.Substring(0, message.Length - 1);
                            Console.SetCursorPosition(": ".Length + message.Length, Console.CursorTop);
                        }
                        if (message.Length == 0) 
                        { 
                            isGoing = false; 
                            onMessage = false; 
                            WriteAllMessages();
                            Console.CursorVisible = false;
                        }
                        break;
                    case (char)13:  // ENTER
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT INTO messages (sender, receiver, message, sended) VALUES (@sender, @receiver, @message, @sended)";
                        cmd.Parameters.Add("@sender", MySqlDbType.Int32).Value = user.UserID;
                        cmd.Parameters.Add("@receiver", MySqlDbType.Int32).Value = partner.UserID;
                        cmd.Parameters.Add("@message", MySqlDbType.VarChar).Value = message;
                        cmd.Parameters.Add("@sended", MySqlDbType.DateTime).Value = DateTime.Now;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        onMessage = false;
                        isGoing = false;
                        Console.CursorVisible = false;
                        break;
                    default:
                        Console.Write(pressedKey);
                        message += pressedKey;
                        break;
                }
            }
        }

        public void Idle()
        {
            int currentMessNum;

            new Thread(() =>
            {
                while (isIdle)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Enter:
                            if (!onMessage)
                            {
                                onMessage = true;
                                StartChat();
                            }
                            break;
                        case ConsoleKey.Escape:
                            isIdle = false;
                            break;
                    }
                }
            })
            { Name = "EnterPressedInIdle_Messages" }.Start();

            while (isIdle)
            {
                if (!onMessage)
                {
                    conn.Open();
                    currentMessNum = 0;

                    cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT COUNT(sender) FROM messages WHERE sender = @sender AND receiver = @receiver";
                    cmd.Parameters.Add("@sender", MySqlDbType.Int32).Value = user.UserID;
                    cmd.Parameters.Add("@receiver", MySqlDbType.Int32).Value = partner.UserID;

                    currentMessNum += int.Parse(cmd.ExecuteScalar().ToString());

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@sender", MySqlDbType.Int32).Value = partner.UserID;
                    cmd.Parameters.Add("@receiver", MySqlDbType.Int32).Value = user.UserID;

                    currentMessNum += int.Parse(cmd.ExecuteScalar().ToString());

                    conn.Close();
                    if (currentMessNum > messagesCount) WriteAllMessages();
                }

                Thread.Sleep(300);
            }
            Console.Clear();
        }
    }

    struct Message
    {
        public int From { get; set; }
        public int To { get; set; }
        public string Content { get; set; }
        public DateTime When { get; set; }
    }
}
