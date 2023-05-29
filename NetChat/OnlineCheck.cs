using MySqlConnector;
using System.IO.IsolatedStorage;

namespace NetChat
{
    internal class OnlineCheck
    {
        User user { get; set; }

        public bool isOnline { get; set; }

        MySqlConnection conn;
        MySqlConnectionStringBuilder stringBuilder;
        MySqlCommand cmd;
        MySqlDataReader reader;

        public OnlineCheck(User user)
        {
            this.user = user;
            this.isOnline = true;

            stringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = "localhost",
                Port = 3306,
                UserID = "root",
                Password = "root",
                Database = "onlinechat"
            };

            conn = new MySqlConnection(stringBuilder.ConnectionString);
        }

        public void Start()
        {
            cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE users SET isonline = @isonline WHERE id = @id";
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = user.UserID;
            cmd.Parameters.Add("@isonline", MySqlDbType.Int32).Value = 1;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            new Thread(() =>
            {
                while (isOnline)
                {
                    DateTime timeStamp = DateTime.Now;

                    cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE users SET last_available = @lasta WHERE id = @id";
                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = user.UserID;
                    cmd.Parameters.Add("@lasta", MySqlDbType.DateTime).Value = timeStamp;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    Thread.Sleep(1000);
                }

            }).Start();
        }

        public void OnCloseEvent(object sender, EventArgs e)
        {
            cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE users SET isonline = @isonline WHERE id = @id";
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = user.UserID;
            cmd.Parameters.Add("@isonline", MySqlDbType.Int32).Value = 0;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
