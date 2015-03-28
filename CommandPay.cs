using Rocket.RocketAPI;
using SDG;
using System;

namespace unturned.ROCKS.Uconomy
{
    public class CommandPay : IRocketCommand
    {
        public string Help
        {
            get { return "Pays a specific player money from your account"; }
        }

        public string Name
        {
            get { return "pay"; }
        }

        public bool RunFromConsole
        {
            get { return false; }
        }

        public void Execute(Steamworks.CSteamID caller, string command)
        {
            string[] commandArray = command.Split('/');

            // 1 = COMMAND username, 2 = amount

            if (commandArray.Length != 2)
            {
                ChatManager.say(caller, Uconomy.Instance.Translate("command_pay_invalid"));
                return;
            }

            SteamPlayer callingPlayer = PlayerTool.getSteamPlayer(caller);
            SteamPlayer otherPlayer;
            if (PlayerTool.tryGetSteamPlayer(commandArray[0], out otherPlayer))
            {
                if (caller.ToString() == otherPlayer.SteamPlayerID.CSteamID.ToString())
                {
                    ChatManager.say(caller, Uconomy.Instance.Translate("command_pay_error_pay_self"));
                    return;
                }

                decimal amount = 0;
                if (!Decimal.TryParse(commandArray[1], out amount) || amount <= 0)
                {
                    ChatManager.say(caller, Uconomy.Instance.Translate("command_pay_error_invalid_amount"));
                    return;
                }

                decimal myBalance = Uconomy.Instance.Database.GetBalance(caller);
                if ((myBalance - amount) <= 0)
                {
                    ChatManager.say(caller, Uconomy.Instance.Translate("command_pay_error_cant_afford"));
                    return;
                }
                else
                {
                    Uconomy.Instance.Database.IncreaseBalance(caller, -amount);
                    ChatManager.say(caller, Uconomy.Instance.Translate("command_pay_private", otherPlayer.SteamPlayerID.CharacterName, amount, Uconomy.Instance.Configuration.MoneyName));
                    Uconomy.Instance.Database.IncreaseBalance(otherPlayer.SteamPlayerID.CSteamID, amount);
                    ChatManager.say(otherPlayer.SteamPlayerID.CSteamID, Uconomy.Instance.Translate("command_pay_other_private", amount, Uconomy.Instance.Configuration.MoneyName, callingPlayer.SteamPlayerID.CharacterName));
                }
            }
            else
            {
                ChatManager.say(caller, Uconomy.Instance.Translate("command_pay_error_player_not_found"));
            }
        }
    }
}
