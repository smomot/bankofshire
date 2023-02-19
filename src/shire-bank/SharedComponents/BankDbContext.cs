using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharedComponents.BankModel;

namespace SharedComponents
{
    public class BankDbContext : DbContext
    {

        protected readonly IConfiguration Configuration;

        /// <summary>
        /// For In memory db - development only
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public BankDbContext(DbContextOptions<BankDbContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// For Sql lite connection
        /// </summary>
        /// <param name="configuration"></param>
        public BankDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("BankDatabase"));
        }
        */

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountOperation> AccountOperations { get; set; }
    }
}
