using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamworks;
using SDG;
using Rocket.RocketAPI;
using System.Reflection;
using Rocket.RocketAPI.Interfaces;
using Rocket.RocketAPI.Managers;

namespace Uconomy
{
    public class Uconomy : RocketPlugin
    {
        public static Configuration Configuration = ConfigurationManager.LoadConfiguration<Configuration>();
     
        string RocketPlugin.Author
        {
            get { return "fr34kyn01535"; }
        }
        
        void RocketPlugin.Load()
        {
            new I18N.West.CP1250(); //Workaround for database encoding issues with mono
            RocketAPI.Commands.RegisterCommand(new CommandPay());
            RocketAPI.Commands.RegisterCommand(new CommandBalance());

            RocketAPI.Events.PlayerConnected += onPlayerConnected;

            Database.CheckSchema();
        }

        static void onPlayerConnected(CSteamID id)
        {
           //setup account
            Database.CheckSetupAccount(id);
        }
    }
}
