using System;
using System.Threading.Tasks;
using Rocket.API.Commands;
using Rocket.API.Economy;
using Rocket.API.Plugins;
using Rocket.API.User;
using Rocket.Core.Commands;
using Rocket.Core.I18N;

namespace fr34kyn01535.Uconomy.Commands
{
    public class CommandPay : ICommand
    {
        public string Name => "pay";
        public string[] Aliases => null;
        public string Syntax => "<player> <amount>";
        public string Summary => "Pays a specific player money from your account";
        public string Description => null;
        public bool SupportsUser(IUser user) => true;
        public IChildCommand[] ChildCommands => null;

        private readonly UconomyPlugin _plugin;
        private readonly IEconomyProvider _economyProvider;

        public CommandPay(IPlugin plugin, IEconomyProvider economyProvider)
        {
            _plugin = (UconomyPlugin)plugin;
            _economyProvider = economyProvider;
        }

        public async Task ExecuteAsync(ICommandContext context)
        {
            if (context.Parameters.Length != 2)
            {
                throw new CommandWrongUsageException();
            }

            var targetUser = await context.Parameters.GetAsync<IUser>(0);
            var caller = context.User;

            if (caller == targetUser)
            {
                await caller.SendLocalizedMessageAsync(_plugin.Translations, "command_pay_error_pay_self");
                return;
            }

            var amount = await context.Parameters.GetAsync<decimal>(1);

            if (caller is IConsole)
            {
                await _economyProvider.AddBalanceAsync(targetUser, amount);
                await targetUser.SendLocalizedMessageAsync(_plugin.Translations, "command_pay_console", amount, _economyProvider.DefaultCurrency.Name);
                return;
            }

            var callerBalance = await _economyProvider.GetBalanceAsync(caller);
            if (callerBalance - amount <= 0)
            {
                await caller.SendLocalizedMessageAsync(_plugin.Translations,"command_pay_error_cant_afford");
                return;
            }

            await _economyProvider.RemoveBalanceAsync(caller, amount);
            await caller.SendLocalizedMessageAsync(_plugin.Translations, 
                "command_pay_private", targetUser.DisplayName, amount, _economyProvider.DefaultCurrency.Name);

            await _economyProvider.AddBalanceAsync(targetUser, amount);
            await targetUser.SendLocalizedMessageAsync(_plugin.Translations,
                "command_pay_other_private", amount, _economyProvider.DefaultCurrency.Name, caller.DisplayName);
        }
    }
}
