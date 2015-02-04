using Rocket;
using Rocket.RocketAPI;
using Rocket.RocketAPI.Events;
using SDG;
using Steamworks;

namespace unturned.ROCKS.Uconomy
{
    public class Uconomy : RocketPlugin<UconomyConfiguration>
    {
        public DatabaseManager Database;
        public static Uconomy Instance;

        protected override void Load()
        {
            Instance = this;
            Database = new DatabaseManager();
            RocketServerEvents.OnPlayerConnected+=Events_OnPlayerConnected;
        }

        private void Events_OnPlayerConnected(Player player)
        {
           //setup account
            Database.CheckSetupAccount(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID);
        }
    }
}
