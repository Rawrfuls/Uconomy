using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace unturned.ROCKS.GlobalBan
{
    public class UconomyConfiguration
    {
        public string DatabaseAddress = "localhost";
        public string DatabaseUsername = "unturned";
        public string DatabasePassword = "password";
        public string DatabaseName = "unturned";
        public string DatabaseTableName = "uconomy";

        public decimal InitialBalance = 30;
        public string MoneyName = "Beer";
    }
}
