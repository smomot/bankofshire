// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using InspectorClient;

internal partial class Program
{
    private static async Task Main(string[] args)
    {
        var logger = LogManager.GetCurrentClassLogger();

        try
        {
            var config = new ConfigurationBuilder()
             .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .Build();

            var serviceHost = config.GetSection("BankAddress").Value;
            using var channel = GrpcChannel.ForAddress(serviceHost);
            using var servicesProvider = new ServiceCollection()

           .AddLogging(loggingBuilder =>
           {
               // configure Logging with NLog
               loggingBuilder.ClearProviders();
               loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
               loggingBuilder.AddNLog(config);
           })
           .AddTransient<InspectionService>().AddTransient<Bank.BankClient>(s => new Bank.BankClient(channel))
           .BuildServiceProvider();

             var runner = servicesProvider.GetRequiredService<InspectionService>();
             await runner.StartInspection();
             await runner.GetFullSummary();
             Console.WriteLine("Press any key to continue...");
             Console.ReadKey();
             await runner.FinishInspection();
            
             Console.WriteLine("Press any key to exit...");
             Console.ReadKey();
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
    }
}

