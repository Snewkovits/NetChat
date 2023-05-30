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

            if (test.Database())
            {
                LoginPanel loginPanel = new LoginPanel();
                User user = loginPanel.Login();
                Chats chats = new Chats(user);
                chats.Open();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("The program can't connect to the database!\nCheck your internet connection or try to contact with a support!\n\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Press something to exit...");
                Console.ReadKey(true);
            }
        }
    }
}