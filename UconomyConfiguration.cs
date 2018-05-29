namespace fr34kyn01535.Uconomy
{
    public class UconomyConfiguration
    {
        public string DatabaseAddress { get; set; } = "localhost";
        public string DatabaseUsername { get; set; } = "unturned";
        public string DatabasePassword { get; set; } = "password";
        public string DatabaseName { get; set; } = "unturned";
        public string DatabaseTableName { get; set; } = "uconomy";
        public int DatabasePort { get; set; } = 3306;
        public decimal InitialBalance { get; set; } = 30;
        public string MoneyName { get; set; } = "Credits";
    }
}
