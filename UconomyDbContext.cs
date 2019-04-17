using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fr34kyn01535.Uconomy.Models;
using Microsoft.EntityFrameworkCore;

namespace fr34kyn01535.Uconomy
{
    public class UconomyDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        private readonly string _connectionString;

        public UconomyDbContext()
        {
            _connectionString = "SERVER=localhost;DATABASE=unturned;UID=root;PASSWORD=;PORT=3306;charset=utf8";
        }
        
        public UconomyDbContext(UconomyPlugin plugin)
        {
            _connectionString = plugin.ConfigurationInstance.MySqlConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString);
        }
    }
}
