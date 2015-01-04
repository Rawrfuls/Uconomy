using Rocket;
using Steamworks;

namespace Uconomy
{
    public class Uconomy : RocketComponent
    {
        public static UconomyConfiguration configuration;
     
        protected override void Load()
        {
            new I18N.West.CP1250(); //Workaround for database encoding issues with mono
            configuration = Configuration.LoadConfiguration<UconomyConfiguration>();
            Database.CheckSchema();
        }

        protected override void onPlayerConnected(CSteamID id)
        {
           //setup account
            Database.CheckSetupAccount(id);
        }
    }
}
