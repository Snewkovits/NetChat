using MySqlConnector;

namespace NetChat
{
    internal class MainChunk
    {
        public static void Main()
        {
            Console.CursorVisible = false;
            Console.Title = "NetChat by Snewkovits";

            MySQL.SetMySql();

            Test test = new Test();
            test.Database();

            LoginPanel loginPanel = new LoginPanel();
            User user = loginPanel.Login();
            Chats chats = new Chats(user);
            chats.Open();
        }
    }
}