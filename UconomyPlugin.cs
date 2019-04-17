using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rocket.API.DependencyInjection;
using Rocket.API.Economy;
using Rocket.API.Eventing;
using Rocket.Core.Player.Events;
using Rocket.Core.Plugins;

namespace fr34kyn01535.Uconomy
{
    public class UconomyPlugin : Plugin<UconomyConfiguration>, IEventListener<PlayerConnectedEvent>
    {
        public UconomyPlugin(IDependencyContainer container) : base("Uconomy", container)
        {
        }

        protected override async Task OnActivate(bool isFromReload)
        {
            using (var context = new UconomyDbContext(this))
            {
                await context.Database.MigrateAsync();
            }

            EventBus.AddEventListener(this, this);
        }

        public async Task HandleEventAsync(IEventEmitter emitter, PlayerConnectedEvent @event)
        {
            await ((UconomyEconomyProvider)Container.Resolve<IEconomyProvider>()).CreateAccountAsync(@event.Player.User);
        }

        public override Dictionary<string, string> DefaultTranslations => new Dictionary<string, string>
        {
            {"command_balance_show", "Your current balance is: {0} {1}"},
            {"command_pay_invalid", "Invalid arguments"},
            {"command_pay_error_pay_self", "You cant pay yourself"},
            {"command_pay_error_invalid_amount", "Invalid amount"},
            {"command_pay_error_cant_afford", "Your balance does not allow this payment"},
            {"command_pay_error_player_not_found", "Failed to find player"},
            {"command_pay_private", "You paid {0} {1} {2}"},
            {"command_pay_console", "You received a payment of {0} {1} "},
            {"command_pay_other_private", "You received a payment of {0} {1} from {2}"}
        };
    }
}