using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace unturned.ROCKS.Uconomy
{
    public class UconomyConfiguration : IRocketPluginConfiguration
    {
        public string DatabaseAddress = "localhost";
        public string DatabaseUsername = "unturned";
        public string DatabasePassword = "password";
        public string DatabaseName = "unturned";
        public string DatabaseTableName = "uconomy";
        public int DatabasePort = 3306;

        public decimal InitialBalance = 30;
        public string MoneyName = "Credits";
    }
}
