using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;

namespace CustomerClient
{
    internal class CustomerFunctionalTest
    {
        private readonly ILogger<CustomerFunctionalTest> _logger;
        Bank.BankClient _client;


        public CustomerFunctionalTest(ILogger<CustomerFunctionalTest> logger, Bank.BankClient client)
        {
            _logger = logger;
            _client = client;
        } 

        public void StartTest()
        {

            ManualResetEvent[] endOfWorkEvents =
            { new ManualResetEvent(false), new ManualResetEvent(false),new ManualResetEvent(false) };
            var historyPrintLock = new object();

                // Customer 1
                new Thread(async () =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    var account = await _client.OpenAccountAsync(new OpenAccountRequest { FirstName = "Henrietta", LastName = "Baggins", DebtLimit = 100.0f });
                    if (account.Id != null)
                    {
                         
                        UInt32Value accountId = new UInt32Value { Value = account.Id.Value };
                        await _client.DepositAsync(new DepositRequest { Account = accountId.Value, Ammount = 500.0f });

                        Thread.Sleep(TimeSpan.FromSeconds(2));

                        await _client.DepositAsync(new DepositRequest { Account = account.Id.Value, Ammount = 500.0f });
                        await _client.DepositAsync(new DepositRequest { Account = account.Id.Value, Ammount = 1000.0f });

                        var withDrawResult = await _client.WithdrawAsync(new WithdrawRequest { Account = account.Id.Value, Ammount = 2000.0f });
                        if (2000.0f != withDrawResult.Value)
                        {
                            _logger.LogWarning("=== Customer 1 === Can't withdraw a valid amount"); 
                        }

                        lock (historyPrintLock)
                        {                          
                            _logger.LogInformation("=== Customer 1 ===");
                            Console.WriteLine();
                            _logger.LogTrace(_client.GetHistory(accountId).Value);
                            Console.WriteLine();
                        }

                        var closeAccountResult = await _client.CloseAccountAsync(accountId);
                        if (!closeAccountResult.Value)
                        {
                            _logger.LogWarning("=== Customer 1 === Failed to close account");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("=== Customer 1 === Failed to open account");
                    }
                    endOfWorkEvents[0].Set();
                }).Start();

                // Customer 2
                new Thread(async () =>
                {

                    var account = await _client.OpenAccountAsync(new OpenAccountRequest { FirstName = "Barbara", LastName = "Tuk", DebtLimit = 50.0f });
                    if (account.Id == null)
                    {
                          _logger.LogWarning("=== Customer 2 === Failed to open account");
                    }
                    else
                    {
                        var anotherAccount = await _client.OpenAccountAsync(new OpenAccountRequest { FirstName = "Barbara", LastName = "Tuk", DebtLimit = 500.0f });

                        if (anotherAccount.Id != null)
                        {
                             _logger.LogWarning("=== Customer 2 === Opened account for the same name twice");
                        }
                      
                         var withdrawResult = await _client.WithdrawAsync(new WithdrawRequest { Account = account.Id.Value, Ammount = 2000.0f });
                          if (50.0f != withdrawResult.Value)
                                      {
                                          _logger.LogWarning("=== Customer 2 === Can only borrow up to debit limit only");
                                      }

                                     Thread.Sleep(TimeSpan.FromSeconds(10));

                                     UInt32Value accountIdConverted = new UInt32Value { Value = account.Id.Value };

                                     if (! ((await _client.CloseAccountAsync(accountIdConverted)).Value))
                                     {
                                       _logger.LogWarning("=== Customer 2 === Can't close the account with outstanding debt");
                                     }

                                      await _client.DepositAsync(new DepositRequest { Account = account.Id.Value, Ammount = 100.0f });
                                      if (!(await _client.CloseAccountAsync(accountIdConverted)).Value)
                                      {
                                          _logger.LogWarning("=== Customer 2 === Can't close the account before clearing all funds");
                                      }

                                      if (50.0f != (await _client.WithdrawAsync(new WithdrawRequest { Account = account.Id.Value, Ammount = 50.0f })).Value)
                                      {
                                          _logger.LogWarning("=== Customer 2 === Can't withdraw a valid amount");
                                      }

                                      lock (historyPrintLock)
                                      {
                                         _logger.LogInformation("=== Customer 2 ===");
                                        Console.WriteLine();
                                        _logger.LogTrace(_client.GetHistory(accountIdConverted).Value);
                                        Console.WriteLine();
                        }

                                      if (!(await _client.CloseAccountAsync(accountIdConverted)).Value)
                                      {
                                          _logger.LogWarning("=== Customer 2 === Failed to close account");
                                      }
                        }

                      endOfWorkEvents[1].Set();
                    
                }).Start();


                // Customer 3
                new Thread(async () =>
                {

                    var account = await _client.OpenAccountAsync(new OpenAccountRequest { FirstName = "Gandalf", LastName = "Grey", DebtLimit = 10000.0f });
                    if (account.Id == null)
                    {
                        _logger.LogWarning("=== Customer 3 === Failed to open account");
                    }
                    else
                    {
                        var toProcess = 200;
                        var resetEvent = new ManualResetEvent(false);

                        for (var i = 0; i < 100; i++)
                        {
                            ThreadPool.QueueUserWorkItem(async stateInfo =>
                            {
                                if ((await _client.WithdrawAsync(new WithdrawRequest { Account = account.Id.Value, Ammount = 10.0f })).Value != 10.0f)
                                {
                                    _logger.LogWarning("=== Customer 3 === Can't withdraw a valid amount!");
                                }

                                if (Interlocked.Decrement(ref toProcess) == 0)
                                {
                                    resetEvent.Set();
                                }
                            });
                        }

                        for (var i = 0; i < 100; i++)
                        {
                            ThreadPool.QueueUserWorkItem(async stateInfo =>
                            {
                                await _client.DepositAsync(new DepositRequest { Account = account.Id.Value, Ammount = 10.0f });
                                if (Interlocked.Decrement(ref toProcess) == 0)
                                {
                                    resetEvent.Set();
                                }
                            });
                        }

                        Thread.Sleep(TimeSpan.FromSeconds(10));

                        resetEvent.WaitOne();
                        UInt32Value accountIdConverted = new UInt32Value { Value = account.Id.Value };

                        lock (historyPrintLock)
                        {
                            _logger.LogInformation("=== Customer 3 ===");
                             Console.WriteLine();
                            _logger.LogTrace(_client.GetHistory(accountIdConverted).Value);
                             Console.WriteLine();
                        }

                    
                        if (!(await _client.CloseAccountAsync(accountIdConverted)).Value)
                        {
                            _logger.LogWarning("=== Customer 3 === Failed to close account");
                        }
                    }
                    endOfWorkEvents[2].Set();
                 }).Start();
                 WaitHandle.WaitAll(endOfWorkEvents);
                
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
        }
        }
    
}
