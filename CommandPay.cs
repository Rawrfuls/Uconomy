using Rocket.RocketAPI;
using SDG;
using System;

namespace unturned.ROCKS.Uconomy
{
    class CommandPay : Command
    {
        public CommandPay()
        {
            base.commandName = "pay";
            base.commandHelp = "Pays a specific player money from your account";
            base.commandInfo = base.commandName + " - " + base.commandHelp;
        }

        protected override void execute(SteamPlayerID caller, string command)
        {
            if (!RocketCommand.IsPlayer(caller)) return;
            string[] commandArray = command.Split('/');

            // 1 = COMMAND username, 2 = amount

            if (commandArray.Length != 2)
            {
                ChatManager.say(caller.CSteamID, Uconomy.Instance.Translate("command_pay_invalid"));
                return;
            }

            SteamPlayer otherPlayer;
            if (SteamPlayerlist.tryGetSteamPlayer(commandArray[0], out otherPlayer))
            {
                if (caller.CSteamID.ToString() == otherPlayer.SteamPlayerID.CSteamID.ToString())
                {
                    ChatManager.say(caller.CSteamID, Uconomy.Instance.Translate("command_pay_error_pay_self"));
                    return;
                }

                decimal amount = 0;
                if (!Decimal.TryParse(commandArray[1], out amount) || amount <= 0)
                {
                    ChatManager.say(caller.CSteamID, Uconomy.Instance.Translate("command_pay_error_invalid_amount"));
                    return;
                }

                decimal myBalance = Uconomy.Instance.Database.GetBalance(caller.CSteamID);
                if ((myBalance - amount) <= 0)
                {
                    ChatManager.say(caller.CSteamID, Uconomy.Instance.Translate("command_pay_error_cant_afford"));
                    return;
                }
                else
                {
                    Uconomy.Instance.Database.IncreaseBalance(caller.CSteamID, -amount);
                    ChatManager.say(caller.CSteamID, Uconomy.Instance.Translate("command_pay_private", otherPlayer.SteamPlayerID.CharacterName, amount, Uconomy.Instance.Configuration.MoneyName));
                    Uconomy.Instance.Database.IncreaseBalance(otherPlayer.SteamPlayerID.CSteamID, amount);
                    ChatManager.say(otherPlayer.SteamPlayerID.CSteamID, Uconomy.Instance.Translate("command_pay_other_private", amount, Uconomy.Instance.Configuration.MoneyName, caller.CharacterName));
                }
            }
            else
            {
                ChatManager.say(caller.CSteamID, Uconomy.Instance.Translate("command_pay_error_player_not_found"));
            }
        }
    }
}
