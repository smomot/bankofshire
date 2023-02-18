using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using ShireBankService.Infrastructure;
using ShireBankService.Services;


var logger = LogManager.GetCurrentClassLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);
    var dataSource = builder.Configuration.GetConnectionString("BankDatabase");
    
    builder.Services.AddGrpc();
    //builder.Services.AddDbContext<BankDbContext>(opt => opt.UseInMemoryDatabase("BankDb"));
    builder.Services.AddDbContext<BankDbContext>(opt => opt.UseSqlite(dataSource));
    builder.Services.AddSingleton<ApplicationState>();
    
    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    var app = builder.Build();
    // Configure the HTTP request pipeline.
    app.MapGrpcService<BankService>();
  
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MigrateDatabase();
app.Run();

}
catch (Exception ex)
{
    // NLog: catch any exception and log it.
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}