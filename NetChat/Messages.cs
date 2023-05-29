using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat
{
    internal class MessagePanel
    {
        User user { get; set; }
        Partner partner { get; set; }

        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataReader reader;

        public MessagePanel(User user, Partner partner)
        {
            this.user = user;
            this.partner = partner;

            MySqlConnectionStringBuilder connBuilder = new MySqlConnectionStringBuilder()
            {
                Server = "localhost",
                Port = 3306,
                UserID = "root",
                Password = "root",
                Database = "onlinechat"
            };

            conn = new MySqlConnection(connBuilder.ConnectionString);
        }

        public void WriteAllMessages()
        {
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
                    When = (DateTime)reader["when"]
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
                    When = (DateTime)reader["when"]
                });
            }
            reader.Close();
            conn.Close();

            foreach (Message message in messages)
            {
                Console.WriteLine(message.Content);
            }

            Console.WriteLine("########################################################");
            messages.SortMessages();

            foreach (Message message in messages)
            {
                Console.WriteLine(message.Content);
            }
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
