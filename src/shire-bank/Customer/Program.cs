using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

using CustomerClient;


internal partial class Program
{
    private static void Main(string[] args)
    {
        var logger = LogManager.GetCurrentClassLogger();

        try
        {
            var config = new ConfigurationBuilder()
             .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .Build();
          
            string serviceHost = config.GetValue<string>("BankAddress"); 
            using var channel = GrpcChannel.ForAddress(serviceHost);
            using var servicesProvider = new ServiceCollection()

           .AddLogging(loggingBuilder =>
           {
               // configure Logging with NLog
               loggingBuilder.ClearProviders();
               loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
               loggingBuilder.AddNLog(config);
           }).
            AddTransient<CustomerFunctionalTest>().AddTransient<Bank.BankClient>(s => new Bank.BankClient(channel))
           .BuildServiceProvider();
            var runner = servicesProvider.GetRequiredService<CustomerFunctionalTest>();
            runner.StartTest();

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



