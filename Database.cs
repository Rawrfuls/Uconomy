using MySql.Data.MySqlClient;
using System;

namespace Uconomy
{
    class Database
    {
        private static MySqlConnection createConnection()
        {
            MySqlConnection connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", Uconomy.configuration.DatabaseAddress, Uconomy.configuration.DatabaseName, Uconomy.configuration.DatabaseUsername, Uconomy.configuration.DatabasePassword));
            return connection;
        }

        /// <summary>
        /// returns the current balance of an account
        /// </summary>
        /// <param name="steamId"></param>
        /// <returns></returns>
        public static decimal GetBalance(string steamId)
        {
            decimal output = 0;
            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select `balance` from `" + Uconomy.configuration.DatabaseTableName + "` where `steamId` = '" + steamId + "';";
            connection.Open();
            object result = command.ExecuteScalar();
            if (result != null) Decimal.TryParse(result.ToString(),out output);
            connection.Close();
            return output;
        }

        /// <summary>
        /// Increasing balance to increaseBy (can be negative)
        /// </summary>
        /// <param name="steamId">steamid of the accountowner</param>
        /// <param name="increaseBy">amount to change</param>
        /// <returns>the new balance</returns>
        public static decimal IncreaseBalance(string steamId, decimal increaseBy)
        {
            decimal output = 0;
            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "update `" + Uconomy.configuration.DatabaseTableName + "` set `balance` = balance + (" + increaseBy + ") where `steamId` = '" + steamId + "'; select `balance` from `" + Uconomy.configuration.DatabaseTableName + "` where `steamId` = '" + steamId + "'";
            connection.Open();
            object result = command.ExecuteScalar();
            if (result != null) Decimal.TryParse(result.ToString(), out output);
            connection.Close();
            return output;
        }

        
        public static void CheckSetupAccount(Steamworks.CSteamID id)
        {
            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();

            int exists = 0;
            command.CommandText = "select count(id) from `" + Uconomy.configuration.DatabaseTableName + "` where `steamId` = '" + id + "';";
            connection.Open();
            object result = command.ExecuteScalar();
            if (result != null) Int32.TryParse(result.ToString(), out exists);
            connection.Close();

            if (exists == 0) {
                command.CommandText = "insert ignore into `" + Uconomy.configuration.DatabaseTableName + "` (balance,steamId,lastUpdated) values(" + Uconomy.configuration.InitialBalance + ",'" + id.ToString() + "',now())";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

        }

        public static void CheckSchema()
        {
            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "show tables like '" + Uconomy.configuration.DatabaseTableName + "'";
            connection.Open();
            object test = command.ExecuteScalar();

            if (test == null)
            {
                command.CommandText = "CREATE TABLE `" + Uconomy.configuration.DatabaseTableName + "` (`id` int(11) NOT NULL AUTO_INCREMENT,`steamId` varchar(32) NOT NULL,`balance` decimal(15,2) NOT NULL DEFAULT '25.00',`lastUpdated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,PRIMARY KEY (`id`,`steamId`)) ";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }


    }
}
