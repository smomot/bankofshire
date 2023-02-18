using Microsoft.EntityFrameworkCore;

namespace ShireBankService.Infrastructure
{
    public class BankDbContext:DbContext
    {

        protected readonly IConfiguration Configuration;



        public BankDbContext(IConfiguration configuration)
        {
            Configuration = configuration;  
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("BankDatabase"));
        }


        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountOperation> AccountOperations { get; set; }
    }
}
