using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG;
using UnityEngine;
using Rocket.RocketAPI;
using System.Web.Script.Serialization;
using Rocket.RocketAPI.Interfaces;

namespace GlobalBan
{
    class CommandUnban : RocketCommand
    {
        public void Execute(SteamPlayerID caller, string command)
        {
            string[] commandArray = command.Split(' ');

            if (commandArray.Length < 2)
            {
                ChatManager.say(caller.CSteamId, "Missing arguments");
                return;
            }

            SteamPlayerID steamPlayerID;
            if (SteamPlayerlist.tryGetPlayer(commandArray[1], out steamPlayerID))
            {
                Database.UnbanPlayer(steamPlayerID.CSteamId.ToString());
                ChatManager.say("Unbanned " + steamPlayerID.IngameName);
            }else{
                ChatManager.say(caller.CSteamId,"Failed to find player");
            }
        }

        public string Name
        {
            get { return "Unban"; }
        }

        public string Help
        {
            get { return "Unbanns a player"; }
        }
    }
}
