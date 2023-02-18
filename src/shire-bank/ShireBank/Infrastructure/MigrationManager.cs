using Microsoft.EntityFrameworkCore;

namespace ShireBankService.Infrastructure
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<BankDbContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        //To do: log migration error.
                        throw;
                    }
                }
            }
            return webApp;
        }
    }
}
