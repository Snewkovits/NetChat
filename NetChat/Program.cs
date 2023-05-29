using MySqlConnector;

namespace NetChat
{
    internal class Program
    {
        public static OnlineCheck oc;

        public static void Main()
        {
            LoginPanel loginPanel = new LoginPanel();
            User user = loginPanel.Login();
            oc = new OnlineCheck(user);
            oc.Start();
            Chats chats = new Chats(user);
            chats.Open();
            oc.isOnline = false;
        }
    }
}