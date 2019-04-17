using System;
using Rocket.API.Economy;

namespace fr34kyn01535.Uconomy
{
    public class UconomyCurrency : IEconomyCurrency
    {
        public UconomyCurrency(string name)
        {
            Name = name;
        }

        public decimal ExchangeTo(decimal amount, IEconomyCurrency targetCurrency)
        {
            throw new NotSupportedException();
        }

        public bool CanExchange(IEconomyCurrency currency)
        {
            throw new NotSupportedException();
        }

        public string Name { get; }
    }
}