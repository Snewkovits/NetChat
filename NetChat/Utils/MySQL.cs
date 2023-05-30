using MySqlConnector;

namespace NetChat
{
    internal class MySQL
    {
        public static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder()
        {
            Server = "mysql.fivemhosting.hu",
            Port = 3306,
            UserID = "u65_G1nwdk4cNP",
            Password = "RT!HBH=Txnb^atLb2Xe=fcLz",
            Database = "s65_onlinechat"
        };

        public static string ConnectionString = builder.ConnectionString;
    }

    /*
      

CREATE DATABASE IF NOT EXISTS `onlinechat` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `onlinechat`;     
     
CREATE TABLE IF NOT EXISTS `messages` (
    `id` int NOT NULL AUTO_INCREMENT,
    `sender` int DEFAULT NULL,
    `receiver` int DEFAULT NULL,
    `message` varchar(250) COLLATE utf8mb4_hungarian_ci DEFAULT NULL,
    `sended` datetime DEFAULT NULL,
    PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

CREATE TABLE IF NOT EXISTS `users` (
    `id` int DEFAULT NULL,
    `username` varchar(50) COLLATE utf8mb4_hungarian_ci DEFAULT NULL,
    `password` varchar(50) COLLATE utf8mb4_hungarian_ci DEFAULT NULL,
    `last_available` datetime DEFAULT NULL,
    `isonline` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;


     */
}
