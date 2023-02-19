using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CustomerClient
{
    /// <summary>
    /// Strings with communicates for log and console
    /// </summary>
    public static class Communicates
    {
        public const string
              //Open account 
              FailedToOpenAccount = "Failed to open account",
              TriedTopenAccountForTheSameNameTwice = "Opened account for the same name twice",
              //Close account
              FailedToCloseAccount = "Failed to close account",
              CloseProblemClearBallanceFirst = "Can't close the account before clearing all funds",
              //Withdraw 
              WihtdrawVallidAmmountProblem = "Can't withdraw a valid amount",
              BorrowLimitOnlyProblem = "Can only borrow up to debit limit only",
              OutstandingDebtProblem = "Can't close the account with outstanding debt";
    }


    internal class CustomerFunctionalTest
    {
        private readonly ILogger<CustomerFunctionalTest> _logger;
        object historyPrintLock = new object();
        Bank.BankClient _client;
        ManualResetEvent[] endOfWorkEvents = { new ManualResetEvent(false) };
        
        public CustomerFunctionalTest(ILogger<CustomerFunctionalTest> logger, Bank.BankClient client)
        {
            _logger = logger;
            _client = client;
        }



        /// <summary>
        /// Customer one method
        /// </summary>
        /// <returns></returns>
        public async Task CutomerOneTest(string customerName)
        {
            await Task.Delay(2);
            var account = await _client.OpenAccountAsync(new OpenAccountRequest { FirstName = "Henrietta", LastName = "Baggins", DebtLimit = 100.0f });
            if (account.Id != null)
            {

                UInt32Value accountId = new UInt32Value { Value = account.Id.Value };
                await _client.DepositAsync(new DepositRequest { Account = accountId.Value, Ammount = 500.0f });

                await Task.Delay(2);

                await _client.DepositAsync(new DepositRequest { Account = account.Id.Value, Ammount = 500.0f });
                await _client.DepositAsync(new DepositRequest { Account = account.Id.Value, Ammount = 1000.0f });
                
                var withDrawResult = await _client.WithdrawAsync(new WithdrawRequest { Account = account.Id.Value, Ammount = 2000.0f });
                if (2000.0f != withDrawResult.Value)
                {
                    _logger.LogWarning("=== "+customerName+" === "+ Communicates.WihtdrawVallidAmmountProblem);
                }

                lock (historyPrintLock)
                {
                    _logger.LogInformation("=== "+customerName+" ===");
                    Console.WriteLine();
                    _logger.LogTrace(_client.GetHistory(accountId).Value);
                    Console.WriteLine();
                }

                var closeAccountResult = await _client.CloseAccountAsync(accountId);
                if (!closeAccountResult.Value)
                {
                    _logger.LogWarning("=== "+customerName+" === "+ Communicates.FailedToCloseAccount);
                }
            }
            else
            {
                _logger.LogWarning("=== "+customerName+" === "+Communicates.FailedToOpenAccount);
            }

        }

        /// <summary>
        /// Customer two method 
        /// </summary>
        /// <returns></returns>
        public async Task CustomerTwoTest(string customerName)
        {
            var account = await _client.OpenAccountAsync(new OpenAccountRequest { FirstName = "Barbara", LastName = "Tuk", DebtLimit = 50.0f });
            if (account.Id == null)
            {
                _logger.LogWarning("=== "+customerName+" === "+Communicates.FailedToOpenAccount);
            }
            else
            {
                var anotherAccount = await _client.OpenAccountAsync(new OpenAccountRequest { FirstName = "Barbara", LastName = "Tuk", DebtLimit = 500.0f });

                if (anotherAccount.Id != null)
                {
                    _logger.LogWarning("=== "+ customerName+ " === "+Communicates.TriedTopenAccountForTheSameNameTwice);
                }

                var withdrawResult = await _client.WithdrawAsync(new WithdrawRequest { Account = account.Id.Value, Ammount = 2000.0f });
                if (50.0f != withdrawResult.Value)
                {
                    _logger.LogWarning("=== "+customerName+ " === " + Communicates.BorrowLimitOnlyProblem);
                }

                await Task.Delay(TimeSpan.FromSeconds(10));

                UInt32Value accountIdConverted = new UInt32Value { Value = account.Id.Value };

                if (!((await _client.CloseAccountAsync(accountIdConverted)).Value))
                {
                    _logger.LogWarning("=== "+customerName+ " === "+Communicates.OutstandingDebtProblem);
                }

                await _client.DepositAsync(new DepositRequest { Account = account.Id.Value, Ammount = 100.0f });
                if (!(await _client.CloseAccountAsync(accountIdConverted)).Value)
                {
                    _logger.LogWarning("===" +customerName+ " === "+Communicates.CloseProblemClearBallanceFirst);
                }

                if (50.0f != (await _client.WithdrawAsync(new WithdrawRequest { Account = account.Id.Value, Ammount = 50.0f })).Value)
                {
                    _logger.LogWarning("=== "+customerName+" === "+Communicates.WihtdrawVallidAmmountProblem);
                }

                lock (historyPrintLock)
                {
                    _logger.LogInformation("=== " +customerName+ " ===");
                    Console.WriteLine();
                    _logger.LogTrace(_client.GetHistory(accountIdConverted).Value);
                    Console.WriteLine();
                }

                if (!(await _client.CloseAccountAsync(accountIdConverted)).Value)
                {
                    _logger.LogWarning("=== " +customerName+ " === "+Communicates.FailedToCloseAccount);
                }
            }
        }

        /// <summary>
        /// Customer 3 method 
        /// </summary>
        /// <returns></returns>
        public async Task CustomerThreeTest(string customerName)
        {
            var account = await _client.OpenAccountAsync(new OpenAccountRequest { FirstName = "Gandalf", LastName = "Grey", DebtLimit = 10000.0f });
            if (account.Id == null)
            {
                _logger.LogWarning("=== "+customerName+" === "+Communicates.FailedToOpenAccount);
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
                            _logger.LogWarning("=== "+customerName+" === "+Communicates.WihtdrawVallidAmmountProblem);
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
                await Task.Delay(TimeSpan.FromSeconds(10));

                resetEvent.WaitOne();
                UInt32Value accountIdConverted = new UInt32Value { Value = account.Id.Value };

                lock (historyPrintLock)
                {
                    _logger.LogInformation("=== "+customerName+ " ===");
                    Console.WriteLine();
                    _logger.LogTrace(_client.GetHistory(accountIdConverted).Value);
                    Console.WriteLine();
                }


                if (!(await _client.CloseAccountAsync(accountIdConverted)).Value)
                {
                    _logger.LogWarning("=== " +customerName+" === "+Communicates.FailedToCloseAccount);
                }
            }
            endOfWorkEvents[0].Set();
        }


        /// <summary>
        /// Main test method 
        /// </summary>
        /// <returns></returns>
        public async Task StartTest()
        {
            Task[] tasks = { CutomerOneTest("CUSTOMER 1"), CustomerTwoTest("CUSTOMER 2"), CustomerThreeTest("CUSTOMER 3") };
            Task.WaitAll(tasks);
            WaitHandle.WaitAll(endOfWorkEvents);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
