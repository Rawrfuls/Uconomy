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

namespace GlobalBan
{
    public class GlobalBan : RocketPlugin
    {
        public static Configuration Configuration = ConfigurationManager.LoadConfiguration<Configuration>();
     
        string RocketPlugin.Author
        {
            get { return "fr34kyn01535"; }
        }
        
        void RocketPlugin.Load()
        {
            new I18N.West.CP1250();
            RocketAPI.Commands.RegisterCommand(new CommandBan());
            RocketAPI.Commands.RegisterCommand(new CommandUnban());

            RocketAPI.Events.PlayerConnected += onPlayerConnected;

            Database.CheckSchema();
        }

        static void onPlayerConnected(CSteamID id)
        {
            try
            {
                string banned = Database.IsBanned(id.ToString());
                if (banned != null)
                {
                    if (banned == "") banned = "You are banned, contact the staff if you feel this is a mistake.";
                    Steam.kick(id, banned);
                }
            }
            catch (Exception ex)
            {
                //Nelson has to fix that....
            }
        }
    }
}
