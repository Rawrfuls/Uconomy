using Rocket;
using Rocket.RocketAPI;
using SDG;
using Steamworks;

namespace unturned.ROCKS.Uconomy
{
    public class Uconomy : RocketPlugin<UconomyConfiguration>
    {
        protected override void Load()
        {
            new I18N.West.CP1250(); //Workaround for database encoding issues with mono
            Database.CheckSchema();
            Events.OnPlayerConnected+=Events_OnPlayerConnected;
        }

        private void Events_OnPlayerConnected(Player player)
        {
           //setup account
            Database.CheckSetupAccount(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID);
        }
    }
}
