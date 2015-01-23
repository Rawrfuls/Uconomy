using MySql.Data.MySqlClient;
using Rocket;
using Rocket.RocketAPI;
using System;

namespace unturned.ROCKS.Uconomy
{
    public class DatabaseManager
    {
        internal DatabaseManager()
        {
            new I18N.West.CP1250(); //Workaround for database encoding issues with mono
            CheckSchema();
        }

        private MySqlConnection createConnection()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", Uconomy.Configuration.DatabaseAddress, Uconomy.Configuration.DatabaseName, Uconomy.Configuration.DatabaseUsername, Uconomy.Configuration.DatabasePassword));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return connection;
        }

        /// <summary>
        /// returns the current balance of an account
        /// </summary>
        /// <param name="steamId"></param>
        /// <returns></returns>
        public decimal GetBalance(Steamworks.CSteamID id)
        {
            decimal output = 0;
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `balance` from `" + Uconomy.Configuration.DatabaseTableName + "` where `steamId` = '" + id.ToString() + "';";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) Decimal.TryParse(result.ToString(), out output);
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return output;
        }

        /// <summary>
        /// Increasing balance to increaseBy (can be negative)
        /// </summary>
        /// <param name="steamId">steamid of the accountowner</param>
        /// <param name="increaseBy">amount to change</param>
        /// <returns>the new balance</returns>
        public decimal IncreaseBalance(Steamworks.CSteamID id, decimal increaseBy)
        {
            decimal output = 0;
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "update `" + Uconomy.Configuration.DatabaseTableName + "` set `balance` = balance + (" + increaseBy + ") where `steamId` = '" + id.ToString() + "'; select `balance` from `" + Uconomy.Configuration.DatabaseTableName + "` where `steamId` = '" + id.ToString() + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) Decimal.TryParse(result.ToString(), out output);
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return output;
        }

        
        public void CheckSetupAccount(Steamworks.CSteamID id)
        {
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                int exists = 0;
                command.CommandText = "select count(id) from `" + Uconomy.Configuration.DatabaseTableName + "` where `steamId` = '" + id + "';";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) Int32.TryParse(result.ToString(), out exists);
                connection.Close();

                if (exists == 0)
                {
                    command.CommandText = "insert ignore into `" + Uconomy.Configuration.DatabaseTableName + "` (balance,steamId,lastUpdated) values(" + Uconomy.Configuration.InitialBalance + ",'" + id.ToString() + "',now())";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

        }

        internal void CheckSchema()
        {
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "show tables like '" + Uconomy.Configuration.DatabaseTableName + "'";
                connection.Open();
                object test = command.ExecuteScalar();

                if (test == null)
                {
                    command.CommandText = "CREATE TABLE `" + Uconomy.Configuration.DatabaseTableName + "` (`id` int(11) NOT NULL AUTO_INCREMENT,`steamId` varchar(32) NOT NULL,`balance` decimal(15,2) NOT NULL DEFAULT '25.00',`lastUpdated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,PRIMARY KEY (`id`,`steamId`)) ";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
