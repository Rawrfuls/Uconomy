using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using SDG;
using System;
using System.Collections.Generic;

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

        public string Syntax
        {
            get { return "<player> <amount>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(RocketPlayer caller, params string[] command)
        {
            if (command.Length != 2)
            {
                RocketChat.Say(caller, Uconomy.Instance.Translate("command_pay_invalid"));
                return;
            }

            RocketPlayer otherPlayer = RocketPlayer.FromName(command[0]);
            if (otherPlayer !=null)
            {
                if (caller == otherPlayer)
                {
                    RocketChat.Say(caller, Uconomy.Instance.Translate("command_pay_error_pay_self"));
                    return;
                }

                decimal amount = 0;
                if (!Decimal.TryParse(command[1], out amount) || amount <= 0)
                {
                    RocketChat.Say(caller, Uconomy.Instance.Translate("command_pay_error_invalid_amount"));
                    return;
                }

                decimal myBalance = Uconomy.Instance.Database.GetBalance(caller.CSteamID);
                if ((myBalance - amount) <= 0)
                {
                    RocketChat.Say(caller, Uconomy.Instance.Translate("command_pay_error_cant_afford"));
                    return;
                }
                else
                {
                    Uconomy.Instance.Database.IncreaseBalance(caller.CSteamID, -amount);
                    RocketChat.Say(caller, Uconomy.Instance.Translate("command_pay_private", otherPlayer.CharacterName, amount, Uconomy.Instance.Configuration.MoneyName));
                    Uconomy.Instance.Database.IncreaseBalance(otherPlayer.CSteamID, amount);
                    RocketChat.Say(otherPlayer.CSteamID, Uconomy.Instance.Translate("command_pay_other_private", amount, Uconomy.Instance.Configuration.MoneyName, caller.CharacterName));
                }
            }
            else
            {
                RocketChat.Say(caller, Uconomy.Instance.Translate("command_pay_error_player_not_found"));
            }
        }
    }
}
