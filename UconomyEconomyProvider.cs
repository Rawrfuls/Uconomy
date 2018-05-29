using System;
using System.Collections.Generic;
using Rocket.API.DependencyInjection;
using Rocket.API.Economy;
using Rocket.API.Plugins;
using Rocket.API.User;

namespace fr34kyn01535.Uconomy
{
    public class UconomyEconomyProvider : IEconomyProvider
    {
        private readonly IDependencyContainer _container;
        private Uconomy Uconomy => (Uconomy)_container.Resolve<IPluginManager>().GetPlugin("uconomy");

        public UconomyEconomyProvider(IDependencyContainer container)
        {
            _container = container;
        }

        public void AddBalance(IIdentity owner, decimal amount, string reason = null)
        {
            Uconomy.Database.IncreaseBalance(owner, amount);
        }

        public bool Transfer(IEconomyAccount source, IEconomyAccount target, decimal amount, string reason = null)
        {
            throw new NotSupportedException();
        }

        public void AddBalance(IEconomyAccount account, decimal amount, string reason = null)
        {
            throw new NotSupportedException();
        }

        public bool RemoveBalance(IIdentity owner, decimal amount, string reason = null)
        {
            Uconomy.Database.IncreaseBalance(owner, -amount);
            return true;
        }

        public bool RemoveBalance(IEconomyAccount account, decimal amount, string reason = null)
        {
            throw new NotSupportedException();
        }

        public void SetBalance(IIdentity owner, decimal amount)
        {
            var currentBalance = GetBalance(owner);
            var toAdd = amount - currentBalance;
            AddBalance(owner, toAdd);
        }

        public void SetBalance(IEconomyAccount account, decimal amount)
        {
            throw new NotSupportedException();
        }

        public bool SupportsNegativeBalance(IIdentity owner)
        {
            return false;
        }

        public bool SupportsNegativeBalance(IEconomyAccount account)
        {
            throw new NotSupportedException();
        }

        public bool CreateAccount(IIdentity owner, string name, out IEconomyAccount account)
        {
            throw new NotSupportedException();
        }

        public bool CreateAccount(IIdentity owner, string name, IEconomyCurrency currency, out IEconomyAccount account)
        {
            throw new NotSupportedException();
        }

        public bool DeleteAccount(IEconomyAccount account)
        {
            throw new NotSupportedException();
        }

        public IEconomyAccount GetAccount(IIdentity owner, string accountName = null)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<IEconomyAccount> GetAccounts(IIdentity owner)
        {
            throw new NotSupportedException();
        }

        public bool SupportsIdentity(IIdentity identity)
        {
            return true;
        }

        public decimal GetBalance(IIdentity identity)
        {
            return Uconomy.Database.GetBalance(identity);
        }

        public IEnumerable<IEconomyCurrency> Currencies => new[]
        {
            DefaultCurrency
        };

        public IEconomyCurrency DefaultCurrency => new UconomyCurrency(Uconomy.ConfigurationInstance.MoneyName);
        public bool SupportsMultipleAccounts => false;
    }
}