using Rocket.RocketAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace unturned.ROCKS.Uconomy
{
    public class UconomyConfiguration : RocketConfiguration
    {
        public string DatabaseAddress;
        public string DatabaseUsername;
        public string DatabasePassword;
        public string DatabaseName;
        public string DatabaseTableName;

        public decimal InitialBalance;
        public string MoneyName;

        public RocketConfiguration DefaultConfiguration
        {
            get
            {
                UconomyConfiguration config = new UconomyConfiguration();
                config.DatabaseAddress = "localhost";
                config.DatabaseUsername = "unturned";
                config.DatabasePassword = "password";
                config.DatabaseName = "unturned";
                config.DatabaseTableName = "uconomy";

                config.InitialBalance = 30;
                config.MoneyName = "Beer";
                return config;
            }
        }
    }
}
