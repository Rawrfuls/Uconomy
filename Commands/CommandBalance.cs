using System.Threading.Tasks;
using Rocket.API.Commands;
using Rocket.API.Economy;
using Rocket.API.Plugins;
using Rocket.API.User;
using Rocket.Core.I18N;

namespace fr34kyn01535.Uconomy.Commands
{
    public class CommandBalance : ICommand
    {
        public string Name => "balance";
        public string[] Aliases => null;
        public string Syntax => null;
        public string Summary => "Shows the current balance";
        public string Description => null;
        public bool SupportsUser(IUser user) => !(user is IConsole);
        public IChildCommand[] ChildCommands => null;

        private readonly UconomyPlugin _plugin;
        private readonly IEconomyProvider _economyProvider;

        public CommandBalance(IPlugin plugin, IEconomyProvider economyProvider)
        {
            _plugin = (UconomyPlugin)plugin;
            _economyProvider = economyProvider;
        }

        public async Task ExecuteAsync(ICommandContext context)
        {
            var balance = await _economyProvider.GetBalanceAsync(context.User);
            await context.User.SendLocalizedMessageAsync(_plugin.Translations, "command_balance_show", balance, _economyProvider.DefaultCurrency.Name);
        }
    }
}
