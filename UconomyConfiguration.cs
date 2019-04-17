namespace fr34kyn01535.Uconomy
{
    public class UconomyConfiguration
    {
        public string MySqlConnectionString { get; set; } = "SERVER=;DATABASE=;UID=;PASSWORD=;PORT=;charset=utf8";
        public decimal InitialBalance { get; set; } = 30;
        public string MoneyName { get; set; } = "Credits";
    }
}
