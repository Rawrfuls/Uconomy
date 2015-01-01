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
    class CommandBan : RocketCommand
    {
        public void Execute(SteamPlayerID caller, string command)
        {
            string[] commandArray = command.Split(' ');

            if (commandArray.Length < 2)
            {
                ChatManager.say(caller.CSteamId, "Missing arguments");
                return;
            }

            string message = "";
            if (commandArray.Length <= 3) {
                for (int i = 2; i < commandArray.Length; i++) {
                    message+=commandArray[i];
                }
            }

            SteamPlayer steamPlayer;
            if (SteamPlayerlist.tryGetSteamPlayer(commandArray[1], out steamPlayer))
            {
                Database.BanPlayer(steamPlayer.SteamPlayerId.CSteamId.ToString(),message);
                ChatManager.say("Banned " + steamPlayer.SteamPlayerId.IngameName + (message == ""?"":(" for " + message)));
                Steam.kick(steamPlayer.SteamPlayerId.CSteamId,message);
            }
            else
            {
                ChatManager.say(caller.CSteamId, "Failed to find player");
            }
        }

        public string Name
        {
            get { return "Ban"; }
        }

        public string Help
        {
            get { return "Banns a player"; }
        }
    }
}
