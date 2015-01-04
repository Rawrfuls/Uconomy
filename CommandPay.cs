using SDG;
using System;

namespace Uconomy
{
    class CommandPay : Command
    {
        public CommandPay()
        {
            base.commandName = "pay";
            base.commandInfo = base.commandHelp = "Pays a specific player money from your account";
        }

        protected override void execute(SteamPlayerID caller, string command)
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
                    ChatManager.say(caller.CSteamId, "You paid " + otherPlayer.SteamPlayerId.IngameName + " " + amount + " " + Uconomy.configuration.MoneyName);
                    Database.IncreaseBalance(otherPlayer.SteamPlayerId.CSteamId.ToString(), amount);
                    ChatManager.say(otherPlayer.SteamPlayerId.CSteamId, "You received a payment of " + amount + " " + Uconomy.configuration.MoneyName + " from " + caller.IngameName);
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
    }
}
