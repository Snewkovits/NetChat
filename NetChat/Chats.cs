using MySqlConnector;

namespace NetChat
{
    internal class Chats
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataReader reader;

        User mainUser;

        bool isMenu;

        /// <summary>
        /// Set everything in Chats class with the user
        /// </summary>
        /// <param name="user">The user who use the app</param>
        public Chats(User user)
        {
            mainUser = user;

            isMenu = false;

            conn = new MySqlConnection(MySQL.ConnectionString);
        }

        /// <summary>
        /// Open the partner list menu.
        /// </summary>
        public void Open()
        {
            isMenu = true;

            Menu(ListAllPeople());
        }
        
        /// <summary>
        /// Get the partner list
        /// </summary>
        /// <returns>All people who registered.</returns>
        public List<Partner> ListAllPeople()
        {
            Console.Clear();
            List<Partner> partners = new List<Partner>();

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, username, last_available FROM users";

            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["username"].ToString() != mainUser.Username)
                {
                    partners.Add(new Partner()
                    {
                        UserID = int.Parse(reader["id"].ToString()),
                        Username = reader["username"].ToString(),
                        LastAvailable = reader["last_available"].ToString()
                    });
                }
            }
            conn.Close();

            return partners;
        }

        /// <summary>
        /// Simulate a menu in console
        /// </summary>
        /// <param name="partners">Partner list</param>
        public void Menu(List<Partner> partners)
        {
            int menuLength = partners.Count;
            int selectedIndex = 0;

            while (isMenu)
            {
                Console.Title = "NetChat by Snewkovits";
                Console.Clear();
                for (int i = 0; i < menuLength; i++)
                {
                    if (selectedIndex == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.Write("* ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"{partners[i].Username}  last seen: {partners[i].LastAvailable}\n");
                }
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        if (selectedIndex < menuLength - 1)
                            selectedIndex++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0)
                            selectedIndex--;
                        break;
                    case ConsoleKey.Enter:
                        MessagePanel mPanel = new MessagePanel(mainUser, partners[selectedIndex]);
                        Console.Clear();
                        mPanel.WriteAllMessages();
                        mPanel.Idle();
                        break;
                    case ConsoleKey.Escape:
                        isMenu = false;
                        MainChunk.Main();
                        break;
                }
            }
        }
    }

    struct Partner
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string LastAvailable { get; set; }
    }
}
