

var logger = LogManager.GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var dataSource = builder.Configuration.GetConnectionString("BankDatabase");
    
    builder.Services.AddGrpc();
    //builder.Services.AddDbContext<BankDbContext>(opt => opt.UseInMemoryDatabase("DB"));

    //Sql lite dbcontext - for and test/production
    builder.Services.AddDbContext<BankDbContext>(opt => opt.UseSqlite(dataSource));

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Services.AddSingleton<ApplicationState>();
    builder.Services.AddScoped<ICustomer, Customer>();
    builder.Services.AddScoped<IInspector, Inspector>();

    var app = builder.Build();
    // Configure the HTTP request pipeline.
    app.MapGrpcService<BankService>();
  
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    app.MigrateDatabase();
    AnsiConsole.Write(new FigletText("Bank of Shire").Color(Color.Gold1));
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