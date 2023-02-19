namespace ShireBankServiceTest
{
    public class InspectorTests
    {
        
        [Fact]
        public async Task GetHistorySuccessfull_ValidResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: "BankDb")
                .Options;

            var configuration = new Mock<IConfiguration>();
            var logger = new Mock<ILogger<ApplicationState>>();
            var loggerForInspector = new Mock<ILogger<Inspector>>();
            var applicationState = new ApplicationState(logger.Object);

            // Use a clean instance of the context to run the test
            using (var context = new BankDbContext(options, configuration.Object))
            {
                Account account = new Account { FirstName = "Test", LastName = "Test", DebtLimit = 100, CreatedAt = DateTime.Now, Ballance = 100 };
                await context.Accounts.AddAsync(account);
                AccountOperation operation = new AccountOperation { AccountId = account.Id, Ammount = 100, OperatedAt = DateTime.Now, ActionType = "Withdraw" };
                await context.AccountOperations.AddAsync(operation);
                await context.SaveChangesAsync();
                //Act 
                Inspector inspectorService = new Inspector(context,applicationState, loggerForInspector.Object);
                var summaryResult = await inspectorService.GetFullSummary();                
                Assert.NotNull(summaryResult);
            }
        }
        
    }
}
