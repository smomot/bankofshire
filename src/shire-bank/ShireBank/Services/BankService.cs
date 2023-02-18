using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShireBankService.Infrastructure;
using System.Text;

namespace ShireBankService.Services
{
    public class BankService : Bank.BankBase
    {
        private readonly ILogger<BankService> _logger;
        ApplicationState _appState;
        BankDbContext _bankDbContext;



        public BankService(ILogger<BankService> logger, ApplicationState appState, BankDbContext bankDbContext)
        {
            _logger = logger;
            _appState = appState;
            _bankDbContext = bankDbContext;
        }

        public override async Task<OpenAccountReply> OpenAccount(OpenAccountRequest request, ServerCallContext context)
        {
            while (_appState.IsSystemLockForClientsOperations)
            {
                await Task.Delay(100);
            }

            OpenAccountReply result = new OpenAccountReply() { Id = null };                       
            Account account = new Account { FirstName = request.FirstName, LastName = request.LastName, CreatedAt = DateTime.Now, DebtLimit = request.DebtLimit };

            var isAccountExist = await _bankDbContext.Accounts.AnyAsync(x => x.LastName == request.LastName && x.FirstName == request.FirstName && !(x.ClosedAt.HasValue));
            if (!isAccountExist)
            {
                await _bankDbContext.Accounts.AddAsync(account);
                await _bankDbContext.SaveChangesAsync();
                result.Id = (uint)account.Id;
                _logger.LogInformation("Account opened for {0} {1} with account number {2} and debt limit {3}", account.FirstName, account.LastName, account.Id, account.DebtLimit);
                return result;
            }
            else
            {
                _logger.LogWarning("Failed to open account for {0} {1}. Active account for requested first and last name exists.", account.FirstName, account.LastName);
            }
            return result;
        }

        public  override async Task<FloatValue> Withdraw(WithdrawRequest request, ServerCallContext context)
        {
            while (_appState.IsSystemLockForClientsOperations)
            {
                await Task.Delay(100);
            }
            FloatValue result = new FloatValue();
            var  account = await _bankDbContext.Accounts.SingleOrDefaultAsync(x => x.Id.Equals((int)request.Account));
            if (account != null)
            {
                if (account.Ballance + account.DebtLimit <= request.Ammount)
                {
                    _logger.LogWarning("Failed to withdraw for {0} {1} ammount of {2}. Insufficent funds.", account.FirstName, account.LastName, request.Ammount);
                }
                else
                {
                    account.Ballance -= request.Ammount;
                    await _bankDbContext.AccountOperations.AddAsync(new AccountOperation { AccountId = account.Id, Ammount = request.Ammount, ActionType = "Withdraw" });
                    await _bankDbContext.SaveChangesAsync();
                    _logger.LogInformation("Withdraw for {0} {1} ammount of {2}.", account.FirstName, account.LastName, request.Ammount);
                    result.Value = request.Ammount;
                }
            }
            return result;
        }
        public override async Task<Empty> Deposit(DepositRequest request, ServerCallContext context)
        {
            while (_appState.IsSystemLockForClientsOperations)
            {
                await Task.Delay(100);
            }

            var account = await _bankDbContext.Accounts.SingleOrDefaultAsync(x => x.Id.Equals((int)request.Account));
            if (account != null)
            {
                account.Ballance += request.Ammount;
                await _bankDbContext.AccountOperations.AddAsync(new AccountOperation { AccountId = account.Id, Ammount = request.Ammount, ActionType = "Deposit" });
                await _bankDbContext.SaveChangesAsync();
                _logger.LogInformation("Deposit for {0} {1} ammount of {2}.", account.FirstName, account.LastName, request.Ammount);
            }
            else
            {
                _logger.LogWarning("Deposit for account {0} unsucessfull. Account not found.", request.Account);
            }
            return new Empty();
        }
        public override async Task<StringValue> GetHistory(UInt32Value request, ServerCallContext context)
        {
            while (_appState.IsSystemLockForClientsOperations)
            {
               await Task.Delay(100);
            }

            StringValue result = new StringValue();
            var account = await _bankDbContext.Accounts.Include(s=>s.AccountOperations).SingleOrDefaultAsync(x => x.Id.Equals((int)request.Value));
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
                    sb.AppendLine("Date :" + item.OperatedAt.ToShortDateString() +" Operation :" + item.ActionType + " Ammount: " + item.Ammount);
                }
                sb.AppendLine(String.Format("-------------------------------"));
                sb.AppendLine(String.Format("[End history]"));
                sb.AppendLine(String.Format("-------------------------------"));
                sb.AppendLine();

                _logger.LogInformation(sb.ToString());
                result.Value = sb.ToString();
            }
            return result;
        }
        public override async Task<BoolValue> CloseAccount(UInt32Value request, ServerCallContext context)
        {
            while (_appState.IsSystemLockForClientsOperations)
            {
                await Task.Delay(100);
            }
            BoolValue result = new BoolValue();
            var account = await _bankDbContext.Accounts.SingleOrDefaultAsync(x => x.Id.Equals((int)request.Value));
            if (account != null && account.Ballance == 0)
            {
                account.ClosedAt = DateTime.Now;
                await _bankDbContext.SaveChangesAsync();
                result.Value = true;
                _logger.LogInformation("Account {0} for {1} {2} closed.", account.Id, account.FirstName, account.LastName);
            }
            else if (account != null && account.Ballance < 0)
            {
               
                _logger.LogWarning("Close account {0} unsucessfull. Outstanding debt.", request.Value);
            }
            else if (account != null && account.Ballance > 0)
            {
                _logger.LogWarning("Close account {0} unsucessfull. Ballance greater than 0.", request.Value);
            }
            else
            {
                _logger.LogWarning("Close account {0} unsucessfull. Account not found.", request.Value);
            }
            return result;
        }
      
        public override Task<StringValue> StartInspection(Empty empty, ServerCallContext context)
        {
            StringValue result = new StringValue();
            _logger.LogWarning("Inspection started");            
            result.Value = _appState.LockSystemForClientOperations();

            return Task.FromResult(result);
        }

        public override Task<StringValue> FinishInspection(Empty empty, ServerCallContext context)
        {
            StringValue result = new StringValue();
            _logger.LogWarning("Inspection finished");
            result.Value = _appState.UnlockSystemForClientOperations();
            return Task.FromResult(result);
        }

        public override async Task<StringValue> GetFullSummary(Empty empty, ServerCallContext context)
        {
            StringValue result = new StringValue();
            var accounts = await _bankDbContext.Accounts.Include(s => s.AccountOperations).ToListAsync();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine(String.Format("-------------------------------"));
            sb.AppendLine(String.Format("[History operations for all accounts]"));
            sb.AppendLine(String.Format("-------------------------------"));

            foreach (var account in accounts)
            {
                sb.AppendLine();
                sb.AppendLine(String.Format("-------------------------------"));
                sb.AppendLine(String.Format("[History operations for {0} {1}]:", account.FirstName, account.LastName));
                sb.AppendLine(String.Format("-------------------------------"));

                var history = account.AccountOperations;
                foreach (var item in history)
                {
                    sb.AppendLine("Date :" + item.OperatedAt.ToShortDateString() + " Operation :" + item.ActionType + " Ammount: " + item.Ammount);
                }

                sb.AppendLine();
                sb.AppendLine(String.Format("-------------------------------"));
                sb.AppendLine(String.Format("[End history]"));
                sb.AppendLine(String.Format("-------------------------------"));
                sb.AppendLine();
            }
            _logger.LogInformation(sb.ToString());
            result.Value = sb.ToString();

            return result;
        }

    }
}