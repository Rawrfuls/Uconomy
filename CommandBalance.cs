using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;
using Rocket.RocketAPI;
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
            if (!RocketCommand.IsPlayer(caller)) return;
            decimal balance = Uconomy.Instance.Database.GetBalance(caller.CSteamID);
            ChatManager.say(caller.CSteamID, Uconomy.Instance.Translate("command_balance_show", balance, Uconomy.Instance.Configuration.MoneyName));
        }

        public string Name
        {
            get { return "balance"; }
        }
    }
}
