using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace ShireBankService.Services
{
    public class Inspector : IInspector {  
        private readonly BankDbContext _dbcontext;
        private readonly ILogger<Inspector> _logger;
        private readonly ApplicationState _applicationState;
        public Inspector(BankDbContext dbcontext, ApplicationState applicationState, ILogger<Inspector> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
            _applicationState = applicationState;
        }
        public Task<string> FinishInspection()
        {
            string result = String.Empty;
            _logger.LogWarning("Inspection finished");
            result = _applicationState.UnlockSystemForClientOperations();
            return Task.FromResult(result);
        }

        public async IAsyncEnumerable<string> GetFullSummary()
        {
            string result = String.Empty;
            var accounts =   _dbcontext.Accounts.Include(s => s.AccountOperations).AsAsyncEnumerable();

            StringBuilder sb = new StringBuilder();
            await foreach (Account account in  accounts) 
            {
               sb.Clear();
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

               yield return sb.ToString();

            }






            //sb.AppendLine();
            //sb.AppendLine(String.Format("-------------------------------"));
            //sb.AppendLine(String.Format("[History operations for all accounts]"));
            //sb.AppendLine(String.Format("-------------------------------"));

            //foreach (var account in accounts)
            //{
            //    sb.AppendLine();
            //    sb.AppendLine(String.Format("-------------------------------"));
            //    sb.AppendLine(String.Format("[History operations for {0} {1}]:", account.FirstName, account.LastName));
            //    sb.AppendLine(String.Format("-------------------------------"));

            //    var history = account.AccountOperations;
            //    foreach (var item in history)
            //    {
            //        sb.AppendLine("Date :" + item.OperatedAt.ToShortDateString() + " Operation :" + item.ActionType + " Ammount: " + item.Ammount);
            //    }

            //    sb.AppendLine();
            //    sb.AppendLine(String.Format("-------------------------------"));
            //    sb.AppendLine(String.Format("[End history]"));
            //    sb.AppendLine(String.Format("-------------------------------"));
            //    sb.AppendLine();
            //}



            //result = sb.ToString();
            //return result;
        }

        public async Task<IEnumerable<Account>> GetItemAsync()
        { 
            return await FetchItems();
        }

        async Task<IEnumerable<Account>> FetchItems()
        {
            return await _dbcontext.Accounts.ToListAsync();
        }

        public  Task<string> StartInspection()
        {
           string result = String.Empty;
           _logger.LogWarning("Inspection started");
           result = _applicationState.LockSystemForClientOperations();
           return Task.FromResult(result); 
        }
    }
}
