using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;
using Rocket.RocketAPI;
using System.Web.Script.Serialization;
using Rocket.RocketAPI.Interfaces;
using MySql.Data.MySqlClient;

namespace GlobalBan
{
    class Database
    {
        private static MySqlConnection createConnection()
        {
            MySqlConnection connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", GlobalBan.Configuration.DatabaseAddress, GlobalBan.Configuration.DatabaseName, GlobalBan.Configuration.DatabaseUsername, GlobalBan.Configuration.DatabasePassword));
            return connection;
        }

        public static string IsBanned(string steamId)
        {
            string output = null;
            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select `banMessage` from `" + GlobalBan.Configuration.DatabaseTableName + "` where `steamId` = '" + steamId + "';";
            connection.Open();
            object result = command.ExecuteScalar();
            if (result != null) output = result.ToString();
            connection.Close();
            return output;
        }

        public static void CheckSchema()
        {
            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "show tables like '" + GlobalBan.Configuration.DatabaseTableName + "'";
            connection.Open();
            object test = command.ExecuteScalar();

            if (test == null)
            {
                command.CommandText = "CREATE TABLE `" + GlobalBan.Configuration.DatabaseTableName + "` (`id` int(11) NOT NULL AUTO_INCREMENT,`steamId` varchar(32) NOT NULL,`banMessage` varchar(255) DEFAULT NULL,`banTime` timestamp NOT NULL DEFAULT `0000-00-00 00:00:00` ON UPDATE CURRENT_TIMESTAMP,PRIMARY KEY (`id`));";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public static void BanPlayer(string steamId, string banMessage)
        {
            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();
            if (banMessage == null) banMessage = "";
            command.CommandText = "insert into `" + GlobalBan.Configuration.DatabaseTableName + "` (`steamId`,`banMessage`,`banTime`) values('" + steamId + "','" + banMessage + "',now());";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void UnbanPlayer(string steamId)
        {
            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "delete from `" + GlobalBan.Configuration.DatabaseTableName + "` where `steamId` = '" + steamId + "';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
