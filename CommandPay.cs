using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;
using Rocket.RocketAPI;
using System.Web.Script.Serialization;
using Rocket.RocketAPI.Interfaces;

namespace Uconomy
{
    class CommandPay : RocketCommand
    {
        public void Execute(SteamPlayerID caller, string command)
        {
            string[] commandArray = command.Split('/');

            // 1 = COMMAND username, 2 = amount

            
            if (commandArray.Length != 3)
            {
                ChatManager.say(caller.CSteamId, "Invalid arguments");
                return;
            }

            string[] commandAndName = commandArray[1].Split(' ');

            if (commandAndName.Length != 2)
            {
                ChatManager.say(caller.CSteamId, "Invalid name");
                return;
            }

            string username = commandAndName[1];

            decimal amount = 0;
            if (!Decimal.TryParse(commandArray[2], out amount) || amount <= 0) {
                ChatManager.say(caller.CSteamId, "Invalid amount");
                return;
            }

            SteamPlayer otherPlayer;
            if (SteamPlayerlist.tryGetSteamPlayer(username, out otherPlayer))
            {
                decimal myBalance = Database.GetBalance(caller.CSteamId.ToString());
                if ((myBalance - amount) <= 0) {
                    ChatManager.say(caller.CSteamId, "Your balance does not allow this payment");
                    return;
                }
                else
                {
                    Database.IncreaseBalance(caller.CSteamId.ToString(), -amount);
                    ChatManager.say(caller.CSteamId, "You paid " + otherPlayer.SteamPlayerId.IngameName + " " + amount + " " + Uconomy.Configuration.MoneyName);
                    Database.IncreaseBalance(otherPlayer.SteamPlayerId.CSteamId.ToString(), amount);
                    ChatManager.say(otherPlayer.SteamPlayerId.CSteamId, "You received a payment of " + amount + " " + Uconomy.Configuration.MoneyName + " from " + caller.IngameName);
                }
            }
            else
            {
                ChatManager.say(caller.CSteamId, "Failed to find player");
            }
        }

        public string Name
        {
            get { return "pay"; }
        }

        public string Help
        {
            get { return "Pays a specific player money from your account"; }
        }
    }
}
