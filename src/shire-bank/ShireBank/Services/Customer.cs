

namespace ShireBankService.Services
{
    public class Customer : ICustomer
    {
        private readonly BankDbContext _dbcontext;
        private readonly ILogger<Customer> _logger; 
        public Customer(BankDbContext dbcontext, ILogger<Customer> logger) 
        {
            _dbcontext = dbcontext;
            _logger = logger;
        }

        public async Task<bool> CloseAccount(uint accountId)
        {
            bool result = false;
            var account =  await _dbcontext.Accounts.SingleOrDefaultAsync(x => x.Id.Equals((int)accountId));
            if (account != null && account.Ballance == 0)
            {
                account.ClosedAt = DateTime.Now;
                await _dbcontext.SaveChangesAsync();
                result = true;
                _logger.LogInformation("Account {0} for {1} {2} closed.", account.Id, account.FirstName, account.LastName);
            }
            else if (account != null && account.Ballance < 0)
            {

                _logger.LogWarning("Close account {0} unsucessfull. Outstanding debt.", accountId);
            }
            else if (account != null && account.Ballance > 0)
            {
                _logger.LogWarning("Close account {0} unsucessfull. Ballance greater than 0.", accountId);
            }
            else
            {
                _logger.LogWarning("Close account {0} unsucessfull. Account not found.", accountId);
            }
            return result;
        }

        public async Task Deposit(uint accountId, float amount)
        {
            var account =  await _dbcontext.Accounts.SingleOrDefaultAsync(x => x.Id.Equals((int)accountId));
            if (account != null)
            {
                 account.Ballance += amount;
                 await _dbcontext.AccountOperations.AddAsync(new AccountOperation { AccountId = account.Id, Ammount = amount, ActionType = "Deposit" });
                 await _dbcontext.SaveChangesAsync();
                 _logger.LogInformation("Deposit for {0} {1} ammount of {2}.", account.FirstName, account.LastName, amount);
            }
            else
            {
                 _logger.LogWarning("Deposit for account {0} unsucessfull. Account not found.", accountId);
            }
        }

        public async Task<string> GetHistory(uint accountId)
        {
            string result = string.Empty;
            var account =  await _dbcontext.Accounts.Include(s => s.AccountOperations).SingleOrDefaultAsync(x => x.Id.Equals((int)accountId));
            if (account != null)
            {
                var history = account.AccountOperations;
                StringBuilder sb = new StringBuilder();

                sb.AppendLine();
                sb.AppendLine(String.Format("-------------------------------"));
                sb.AppendLine(String.Format("[History operations for {0} {1}]:", account.FirstName, account.LastName));
                sb.AppendLine(String.Format("-------------------------------"));

                foreach (var item in history)
                {
                    sb.AppendLine("Date :" + item.OperatedAt.ToShortDateString() + " Operation :" + item.ActionType + " Ammount: " + item.Ammount);
                }
                sb.AppendLine(String.Format("-------------------------------"));
                sb.AppendLine(String.Format("[End history]"));
                sb.AppendLine(String.Format("-------------------------------"));
                sb.AppendLine();

                _logger.LogInformation(sb.ToString());
                result = sb.ToString();
            }
            return result;
        }

        public async Task<uint?> OpenAccount(string firstName, string lastName, float debtLimit)
        {
            uint? result = null;
            var isAccountExist =  await _dbcontext.Accounts.AnyAsync(x => x.LastName == lastName && x.FirstName == firstName && !(x.ClosedAt.HasValue));
            if (!isAccountExist)
            {
                Account account = new Account { FirstName = firstName, LastName = lastName, CreatedAt = DateTime.Now, DebtLimit = debtLimit };

                await _dbcontext.Accounts.AddAsync(account);
                await _dbcontext.SaveChangesAsync();
                result = (uint)account.Id;
                _logger.LogInformation("Account opened for {0} {1} with account number {2} and debt limit {3}", firstName, lastName, account.Id, debtLimit);
                return result;
            }
            else
            {
                _logger.LogWarning("Failed to open account for {0} {1}. Active account for requested first and last name exists.", firstName, lastName);
            }
            return result;
        }

        public async Task<float?> Withdraw(uint accountId, float amount)
        {
            float? result = null;
            var _account =   await _dbcontext.Accounts.SingleOrDefaultAsync(x => x.Id.Equals((int)accountId));
            if (_account != null)
            {
                //Insuficient funds 
                if (_account.Ballance + _account.DebtLimit < amount)
                {
                    _logger.LogWarning("Failed to withdraw for {0} {1} ammount of {2}. Insufficent funds.", _account.FirstName, _account.LastName, amount);
                    return null;
                }
                else
                {
                    _account.Ballance -= amount;
                     await _dbcontext.AccountOperations.AddAsync(new AccountOperation { AccountId = _account.Id, Ammount = amount, ActionType = "Withdraw" });
                     await _dbcontext.SaveChangesAsync();
                    result = amount;
                    _logger.LogInformation("Withdraw for {0} {1} ammount of {2}.", _account.FirstName, _account.LastName, amount);
                }
            }
            return result;
        }
    }
}
