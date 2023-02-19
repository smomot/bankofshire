// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using InspectorClient;
using Spectre.Console;
using Grpc.Core;

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
            
             AnsiConsole.Write(new FigletText("Bank of Shire").Color(Color.Green));
             var inspection = servicesProvider.GetRequiredService<InspectionService>();
             await inspection.StartInspection();
             await inspection.GetFullSummary();
             Console.WriteLine("Press any key to continue...");
             Console.ReadKey();
             await inspection.FinishInspection();
            
             Console.WriteLine("Press any key to exit...");
             Console.ReadKey();
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
        {
            logger.Error("Bank of shire service is currenty offline!");         
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

