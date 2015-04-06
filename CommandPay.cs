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

        public void Execute(RocketPlayer caller, string command)
        {
            string[] commandArray = command.Split('/');

            // 1 = COMMAND username, 2 = amount

            if (commandArray.Length != 2)
            {
                RocketChatManager.Say(caller, Uconomy.Instance.Translate("command_pay_invalid"));
                return;
            }

            RocketPlayer otherPlayer = RocketPlayer.FromName(commandArray[0]);
            if (otherPlayer !=null)
            {
                if (caller == otherPlayer)
                {
                    RocketChatManager.Say(caller, Uconomy.Instance.Translate("command_pay_error_pay_self"));
                    return;
                }

                decimal amount = 0;
                if (!Decimal.TryParse(commandArray[1], out amount) || amount <= 0)
                {
                    RocketChatManager.Say(caller, Uconomy.Instance.Translate("command_pay_error_invalid_amount"));
                    return;
                }

                decimal myBalance = Uconomy.Instance.Database.GetBalance(caller.CSteamID);
                if ((myBalance - amount) <= 0)
                {
                    RocketChatManager.Say(caller, Uconomy.Instance.Translate("command_pay_error_cant_afford"));
                    return;
                }
                else
                {
                    Uconomy.Instance.Database.IncreaseBalance(caller.CSteamID, -amount);
                    RocketChatManager.Say(caller, Uconomy.Instance.Translate("command_pay_private", otherPlayer.CharacterName, amount, Uconomy.Instance.Configuration.MoneyName));
                    Uconomy.Instance.Database.IncreaseBalance(otherPlayer.CSteamID, amount);
                    RocketChatManager.Say(otherPlayer.CSteamID, Uconomy.Instance.Translate("command_pay_other_private", amount, Uconomy.Instance.Configuration.MoneyName, caller.CharacterName));
                }
            }
            else
            {
                RocketChatManager.Say(caller, Uconomy.Instance.Translate("command_pay_error_player_not_found"));
            }
        }
    }
}
