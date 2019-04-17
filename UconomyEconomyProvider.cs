using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fr34kyn01535.Uconomy.Models;
using Microsoft.EntityFrameworkCore;
using Rocket.API.Commands;
using Rocket.API.DependencyInjection;
using Rocket.API.Economy;
using Rocket.API.Plugins;
using Rocket.API.User;

namespace fr34kyn01535.Uconomy
{
    public class UconomyEconomyProvider : IEconomyProvider
    {
        private readonly IDependencyContainer _container;
        private UconomyPlugin Plugin => (UconomyPlugin) _container.Resolve<IPluginLoader>().Plugins.FirstOrDefault(p => p.Name == "Uconomy");

        public UconomyEconomyProvider(IDependencyContainer container)
        {
            _container = container;
        }

        public async Task AddBalanceAsync(IUser owner, decimal amount, string reason = null)
        {
            using (var db = new UconomyDbContext(Plugin))
            {
                var account = await db.Accounts.FirstOrDefaultAsync(a => a.Id == owner.Id);
                account.Balance += amount;
                account.LastUpdated = DateTime.Now;
                await db.SaveChangesAsync();
            }
        }

        public Task<bool> TransferAsync(IEconomyAccount source, IEconomyAccount target, decimal amount, string reason = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddBalanceAsync(IEconomyAccount account, decimal amount, string reason = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveBalanceAsync(IUser owner, decimal amount, string reason = null)
        {
            await AddBalanceAsync(owner, -amount);
            return true;
        }

        public Task<bool> RemoveBalanceAsync(IEconomyAccount account, decimal amount, string reason = null)
        {
            throw new NotImplementedException();
        }

        public async Task SetBalanceAsync(IUser owner, decimal amount)
        {
            using (var db = new UconomyDbContext(Plugin))
            {
                var account = await db.Accounts.FirstOrDefaultAsync(a => a.Id == owner.Id);
                account.Balance = amount;
                account.LastUpdated = DateTime.Now;
                await db.SaveChangesAsync();
            }
        }

        public Task SetBalanceAsync(IEconomyAccount account, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SupportsNegativeBalanceAsync(IEconomyAccount account)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAccountAsync(IUser owner, string name, out IEconomyAccount account)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAccountAsync(IUser owner, string name, IEconomyCurrency currency, out IEconomyAccount account)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAccountAsync(IEconomyAccount account)
        {
            throw new NotImplementedException();
        }

        public Task<IEconomyAccount> GetAccountAsync(IUser owner, string accountName = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEconomyAccount>> GetAccountsAsync(IUser owner)
        {
            throw new NotImplementedException();
        }

        public bool SupportsUser(IUser user)
        {
            return !(user is IConsole);
        }

        public async Task<decimal> GetBalanceAsync(IUser user)
        {
            using (var db = new UconomyDbContext(Plugin))
            {
                var account = await db.Accounts.FirstOrDefaultAsync(a => a.Id == user.Id);
                return account.Balance;
            }
        }

        public IEnumerable<IEconomyCurrency> Currencies => throw new NotSupportedException();
        public IEconomyCurrency DefaultCurrency => new UconomyCurrency(Plugin.ConfigurationInstance.MoneyName);
        public bool SupportsMultipleAccounts => false;

        public async Task CreateAccountAsync(IUser user)
        {
            using (var db = new UconomyDbContext(Plugin))
            {
                if (!await db.Accounts.AnyAsync(a => a.Id == user.Id))
                {
                    var account = new Account(user.Id, Plugin.ConfigurationInstance.InitialBalance);
                    await db.Accounts.AddAsync(account);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}