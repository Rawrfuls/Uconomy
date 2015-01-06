using SDG;
using System;

namespace unturned.ROCKS.GlobalBan
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
                ChatManager.say(caller.CSteamID, "Invalid arguments");
                return;
            }

            string[] commandAndName = commandArray[1].Split(' ');

            if (commandAndName.Length != 2)
            {
                ChatManager.say(caller.CSteamID, "Invalid name");
                return;
            }

            string username = commandAndName[1];

            decimal amount = 0;
            if (!Decimal.TryParse(commandArray[2], out amount) || amount <= 0) {
                ChatManager.say(caller.CSteamID, "Invalid amount");
                return;
            }

            SteamPlayer otherPlayer;
            if (SteamPlayerlist.tryGetSteamPlayer(username, out otherPlayer))
            {
                decimal myBalance = Database.GetBalance(caller.CSteamID.ToString());
                if ((myBalance - amount) <= 0) {
                    ChatManager.say(caller.CSteamID, "Your balance does not allow this payment");
                    return;
                }
                else
                {
                    Database.IncreaseBalance(caller.CSteamID.ToString(), -amount);
                    ChatManager.say(caller.CSteamID, "You paid " + otherPlayer.SteamPlayerID.CharacterName + " " + amount + " " + Uconomy.Configuration.MoneyName);
                    Database.IncreaseBalance(otherPlayer.SteamPlayerID.CSteamID.ToString(), amount);
                    ChatManager.say(otherPlayer.SteamPlayerID.CSteamID, "You received a payment of " + amount + " " + Uconomy.Configuration.MoneyName + " from " + caller.CharacterName);
                }
            }
            else
            {
                ChatManager.say(caller.CSteamID, "Failed to find player");
            }
        }

        public string Name
        {
            get { return "pay"; }
        }
    }
}
