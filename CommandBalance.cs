using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;namespace Uconomy
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
            decimal balance = Database.GetBalance(caller.CSteamId.ToString());
            ChatManager.say(caller.CSteamId, "Your current balance is: " + balance + " " + Uconomy.configuration.MoneyName);
        }

        public string Name
        {
            get { return "balance"; }
        }
    }
}
