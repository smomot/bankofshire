using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using CustomerClient;
using Spectre.Console;
using Grpc.Core;

internal partial class Program
{
    static async Task Main(string[] args)
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

            AnsiConsole.Write(new FigletText("Bank of Shire").Color(Color.Green));
            var functionalTest = servicesProvider.GetRequiredService<CustomerFunctionalTest>();
            await functionalTest.StartTest();
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
        {
            logger.Error("Bank of shire service is currenty offline!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null && ex.InnerException.GetType() == typeof(RpcException) && ((RpcException)ex.InnerException).StatusCode == StatusCode.Unavailable)
            {
                logger.Error("Bank of shire service is currenty offline!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            else
            {

                // NLog: catch any exception and log it.
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
           
        }
        finally
        {
            LogManager.Shutdown();
        }
    }
}



