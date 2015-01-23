using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;
namespace unturned.ROCKS.Uconomy
{
    class CommandBalance : Command
    {
        public CommandBalance()
        {
            base.commandName = "balance";
            base.commandInfo = base.commandHelp = "Shows the current balance";
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            decimal balance = Database.GetBalance(caller.CSteamID.ToString());
            ChatManager.say(caller.CSteamID, "Your current balance is: " + balance + " " + Uconomy.Configuration.MoneyName);
        }

        public string Name
        {
            get { return "balance"; }
        }
    }
}
