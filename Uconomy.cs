using Rocket.Core.Plugins;
using System.Collections.Generic;
using Rocket.API.DependencyInjection;
using Rocket.API.Eventing;
using Rocket.Core.Player;
using Rocket.Core.Player.Events;

namespace fr34kyn01535.Uconomy
{
    public class Uconomy : Plugin<UconomyConfiguration>, IEventListener<PlayerConnectedEvent>
    {
        public DatabaseManager Database;

        protected override void OnLoad(bool isFromReload)
        {
            Database = new DatabaseManager(this, Logger);
            EventManager.AddEventListener(this, this);
        }

        public Uconomy(IDependencyContainer container) : base("Uconomy", container)
        {
        }

        public void HandleEvent(IEventEmitter emitter, PlayerConnectedEvent @event)
        {
            Database.CheckSetupAccount(@event.Player.GetUser());
        }

        public override Dictionary<string, string> DefaultTranslations => new Dictionary<string, string>
        {
            {"command_balance_show","Your current balance is: {0} {1}"},
            {"command_pay_invalid","Invalid arguments"},
            {"command_pay_error_pay_self","You cant pay yourself"},
            {"command_pay_error_invalid_amount","Invalid amount"},
            {"command_pay_error_cant_afford","Your balance does not allow this payment"},
            {"command_pay_error_player_not_found","Failed to find player"},
            {"command_pay_private","You paid {0} {1} {2}"},
            {"command_pay_console","You received a payment of {0} {1} "},
            {"command_pay_other_private","You received a payment of {0} {1} from {2}"},
        };
    }
}
