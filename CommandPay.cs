using System;
using Rocket.API.Commands;
using Rocket.API.Plugins;
using Rocket.API.User;
using Rocket.Core.Commands;
using Rocket.Core.I18N;

namespace fr34kyn01535.Uconomy
{
    public class CommandPay : ICommand
    {
        private readonly Uconomy _uconomy;

        public CommandPay(IPlugin plugin)
        {
            _uconomy = (Uconomy)plugin;
        }

        public string Name => "pay";
        public string[] Aliases => null;
        public string Summary => "Pays a specific player money from your account";
        public string Description => null;
        public string Permission => null;
        public string Syntax => "<player> <amount>";
        public IChildCommand[] ChildCommands => null;

        public bool SupportsUser(Type user)
        {
            return true;
        }

        public void Execute(ICommandContext context)
        {
            if (context.Parameters.Length != 2)
            {
                throw new CommandWrongUsageException();
            }

            IUser otherUser = context.Parameters.Get<IUser>(0);
            var caller = context.User;

            if (caller == otherUser)
            {
                caller.SendLocalizedMessage(_uconomy.Translations, "command_pay_error_pay_self");
                return;
            }

            decimal amount = context.Parameters.Get<decimal>(1);

            if (caller is IConsole)
            {
                _uconomy.Database.IncreaseBalance(otherUser, amount);
                otherUser?.SendLocalizedMessage(_uconomy.Translations, "command_pay_console", amount,
                    _uconomy.ConfigurationInstance.MoneyName);
                return;
            }

            decimal myBalance = _uconomy.Database.GetBalance(caller);
            if ((myBalance - amount) <= 0)
            {
                caller.SendLocalizedMessage(_uconomy.Translations,
                    "command_pay_error_cant_afford");
                return;
            }

            _uconomy.Database.IncreaseBalance(caller, -amount);
            caller.SendLocalizedMessage(_uconomy.Translations,
                "command_pay_private",
                    otherUser.Name, amount,
                    _uconomy.ConfigurationInstance.MoneyName);

            _uconomy.Database.IncreaseBalance(otherUser, amount);
            otherUser.SendLocalizedMessage(_uconomy.Translations,
                "command_pay_other_private", amount,
                _uconomy.ConfigurationInstance.MoneyName, caller.Name);
        }
    }
}
