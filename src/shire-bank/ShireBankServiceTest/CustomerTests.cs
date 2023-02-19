namespace ShireBankServiceTest
{

    public class CustomerTests
    {
        #region Close Account
        [Fact]
        public async Task CloseAccountUnsucessfull_OutstandingDebt()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: "BankDb")
                .Options;

            //IConfiguration configuration = new ConfigurationBuilder()
            var configuration = new Mock<IConfiguration>();
            var logger = new Mock<ILogger<Customer>>();

            // Use a clean instance of the context to run the test
            using (var context = new BankDbContext(options, configuration.Object))
            {
                Account account = new Account { FirstName = "Test", LastName = "Test", DebtLimit = 100, CreatedAt = DateTime.Now, Ballance = -100 };
                await context.Accounts.AddAsync(account);
                await context.SaveChangesAsync();
                //Act 
                Customer customerService = new Customer(context,logger.Object);
                var closeResult = await customerService.CloseAccount((uint)account.Id);
                Assert.False(closeResult);
            }
        }
        
        [Fact]
        public async Task CloseAccountUnsucessfull_BallanceGreaterThan_0()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: "BankDb")
                .Options;

            //IConfiguration configuration = new ConfigurationBuilder()
            var configuration = new Mock<IConfiguration>();
            var logger = new Mock<ILogger<Customer>>();

            // Use a clean instance of the context to run the test
            using (var context = new BankDbContext(options, configuration.Object))
            {
                Account account = new Account { FirstName = "Test", LastName = "Test", DebtLimit = 100, CreatedAt = DateTime.Now, Ballance = 100 };
                await context.Accounts.AddAsync(account);
                await context.SaveChangesAsync();
                //Act 
                Customer customerService = new Customer(context,logger.Object);
                var closeResult = await customerService.CloseAccount((uint)account.Id);
                Assert.False(closeResult);
            }
        }
        
        [Fact]
        public async Task CloseAccountUnsucessfull_AccountNotFound()
        {
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: "BankDb")
                .Options;

            //IConfiguration configuration = new ConfigurationBuilder()
            var configuration = new Mock<IConfiguration>();
            var logger = new Mock<ILogger<Customer>>();

            // Use a clean instance of the context to run the test
            using (var context = new BankDbContext(options, configuration.Object))
            {
                //Act 
                Customer customerService = new Customer(context, logger.Object);
                var closeResult = await customerService.CloseAccount(10);
                Assert.False(closeResult);
            }
        }
        #endregion

        
        #region Withdraw 
        [Fact]
        public async Task WithdrawUnsucessfull_InsufficcientFunds()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: "BankDb")
                .Options;

            //IConfiguration configuration = new ConfigurationBuilder()
            var configuration = new Mock<IConfiguration>();
            var logger = new Mock<ILogger<Customer>>();

            // Use a clean instance of the context to run the test
            using (var context = new BankDbContext(options, configuration.Object))
            {
                Account account = new Account { FirstName = "Test", LastName = "Test", DebtLimit = 100, CreatedAt = DateTime.Now, Ballance = 100 };
                await context.Accounts.AddAsync(account);
                await context.SaveChangesAsync();
                //Act 
                Customer customerService = new Customer(context, logger.Object);
                var withdrawResult = await customerService.Withdraw((uint)account.Id, 201);
                Assert.Null(withdrawResult);
            }
        }
    
        [Fact]
        public async Task WithdrawUnsucessfull_AccountNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: "BankDb")
                .Options;

            //IConfiguration configuration = new ConfigurationBuilder()
            var configuration = new Mock<IConfiguration>();
            var logger = new Mock<ILogger<Customer>>();

            // Use a clean instance of the context to run the test
            using (var context = new BankDbContext(options, configuration.Object))
            {
                
                Account account = new Account { FirstName = "Test", LastName = "Test", DebtLimit = 100, CreatedAt = DateTime.Now, Ballance = 100 };
                await context.Accounts.AddAsync(account);
                await context.SaveChangesAsync();

                //Act 

                //Add another account with the same name
                Customer customerService = new Customer(context, logger.Object);
                var openaccountResult = await customerService.OpenAccount("Test", "Test",100);
                Assert.Null(openaccountResult);
            }
        }
        #endregion
        
        #region Open Account

        [Fact]
        public async Task OpenAccountUnsucessfull_AccountAlreadyExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: "BankDb")
                .Options;

            //IConfiguration configuration = new ConfigurationBuilder()
            var configuration = new Mock<IConfiguration>();
            var logger = new Mock<ILogger<Customer>>();

            // Use a clean instance of the context to run the test
            using (var context = new BankDbContext(options, configuration.Object))
            {
                //Act 
                Customer customerService = new Customer(context, logger.Object);
                var withdrawResult = await customerService.Withdraw(10, 10);
                Assert.Null(withdrawResult);
            }
        }
        #endregion
    }
}