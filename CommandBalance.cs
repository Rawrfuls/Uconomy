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
    class CommandBalance : RocketCommand
    {
        public void Execute(SteamPlayerID caller, string command)
        {
            decimal balance = Database.GetBalance(caller.CSteamId.ToString());
            ChatManager.say(caller.CSteamId, "Your current balance is: " + balance + " " + Uconomy.Configuration.MoneyName);
        }

        public string Name
        {
            get { return "balance"; }
        }

        public string Help
        {
            get { return "Shows the current balance"; }
        }
    }
}
