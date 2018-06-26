using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using Rocket.API.Logging;
using Rocket.API.User;

namespace fr34kyn01535.Uconomy
{
    public class DatabaseManager
    {
        private readonly Uconomy _uconomy;
        private readonly ILogger _logger;

        internal DatabaseManager(Uconomy uconomy, ILogger logger)
        {
            _logger = logger;
            _uconomy = uconomy;
#if NET35
            new I18N.West.CP1250(); //Workaround for database encoding issues with mono
#endif
            CheckSchema();
        }

        private MySqlConnection CreateConnection()
        {
            MySqlConnection connection = null;
            try
            {
                if (_uconomy.ConfigurationInstance.DatabasePort == 0)
                    _uconomy.ConfigurationInstance.DatabasePort = 3306;
                connection = new MySqlConnection(
                    $"SERVER={_uconomy.ConfigurationInstance.DatabaseAddress};" +
                    $"DATABASE={_uconomy.ConfigurationInstance.DatabaseName};" +
                    $"UID={_uconomy.ConfigurationInstance.DatabaseUsername};" +
                    $"PASSWORD={_uconomy.ConfigurationInstance.DatabasePassword};" +
                    $"PORT={_uconomy.ConfigurationInstance.DatabasePort};");
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex);
            }
            return connection;
        }

        public decimal GetBalance(IIdentity identity)
        {
            var id = identity.IdentityType + ":" + identity.Id;

            decimal output = 0;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `balance` from `" + _uconomy.ConfigurationInstance.DatabaseTableName + "` where `steamId` = '" + id + "';";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                    Decimal.TryParse(result.ToString(), out output);
                connection.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex);
            }
            return output;
        }

        public decimal IncreaseBalance(IIdentity identity, decimal increaseBy)
        {
            var id = identity.IdentityType + ":" + identity.Id;

            decimal output = 0;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "update `" + _uconomy.ConfigurationInstance.DatabaseTableName + "` set `balance` = balance + (" + increaseBy + ") where `steamId` = '" + id + "'; select `balance` from `" + _uconomy.ConfigurationInstance.DatabaseTableName + "` where `steamId` = '" + id + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) Decimal.TryParse(result.ToString(), out output);
                connection.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex);
            }
            return output;
        }


        public void CheckSetupAccount(IIdentity identity)
        {
            var id = identity.IdentityType + ":" + identity.Id;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                int exists = 0;
                command.CommandText = "SELECT EXISTS(SELECT 1 FROM `" + _uconomy.ConfigurationInstance.DatabaseTableName + "` WHERE `steamId` ='" + id + "' LIMIT 1);";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) Int32.TryParse(result.ToString(), out exists);
                connection.Close();

                if (exists == 0)
                {
                    command.CommandText = "insert ignore into `" + _uconomy.ConfigurationInstance.DatabaseTableName + "` (balance,steamId,lastUpdated) values(" + _uconomy.ConfigurationInstance.InitialBalance + ",'" + id + "',now())";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex);
            }
        }

        internal void CheckSchema()
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "show tables like '" + _uconomy.ConfigurationInstance.DatabaseTableName + "'";
                connection.Open();
                object test = command.ExecuteScalar();

                if (test == null)
                {
                    command.CommandText = "CREATE TABLE `" + _uconomy.ConfigurationInstance.DatabaseTableName + "` (`steamId` varchar(32) NOT NULL,`balance` decimal(15,2) NOT NULL DEFAULT '25.00',`lastUpdated` timestamp NOT NULL DEFAULT NOW() ON UPDATE CURRENT_TIMESTAMP,PRIMARY KEY (`steamId`)) ";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex);
            }
        }
    }
}
