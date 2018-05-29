using System;
using System.Collections.Generic;
using Rocket.API.Commands;
using Rocket.API.Plugins;
using Rocket.Core.I18N;

namespace fr34kyn01535.Uconomy
{
    public class CommandBalance : ICommand
    {
        private readonly Uconomy _uconomy;

        public CommandBalance(IPlugin plugin)
        {
            _uconomy = (Uconomy) plugin;
        }

        public string Name => "balance";
        public string[] Aliases => null;
        public string Summary => "Shows the current balance";
        public string Description => null;
        public string Permission => null;
        public string Syntax => "";
        public IChildCommand[] ChildCommands => null;

        public bool SupportsUser(Type user)
        {
            return !typeof(IConsole).IsAssignableFrom(user);
        }

        public void Execute(ICommandContext context)
        {
            decimal balance = _uconomy.Database.GetBalance(context.User);
            context.User.SendLocalizedMessage(_uconomy.Translations, "command_balance_show", balance, _uconomy.ConfigurationInstance.MoneyName);
        }
    }
}
