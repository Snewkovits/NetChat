using MySqlConnector;

namespace NetChat
{
    internal class Program
    {
        public static OnlineCheck oc;

        public static void Main()
        {
            Console.CursorVisible = false;
            Console.Title = "OnlineChat by Snewkovits";

            Test test = new Test();
            test.Database();

            LoginPanel loginPanel = new LoginPanel();
            User user = loginPanel.Login();
            Chats chats = new Chats(user);
            chats.Open();
        }
    }
}