

namespace ShireBankService.Services
{
    public class BankService : Bank.BankBase
    {
        ApplicationState _appState;
        ICustomer _customerService;
        IInspector _inspectorService;

        public BankService( 
                           BankDbContext bankDbContext,
                           ApplicationState applicationState,
                           ICustomer customerService, 
                           IInspector inspectorService
                
            )
        {
            _customerService = customerService;
            _inspectorService= inspectorService;
            _appState = applicationState;

        }

        public override async Task<OpenAccountReply> OpenAccount(OpenAccountRequest request, ServerCallContext context)
        {
            while (_appState.IsSystemLockForClientsOperations)
            {
                await Task.Delay(100);
            }
            OpenAccountReply result = new OpenAccountReply() { Id = null };
            var accountId = await _customerService.OpenAccount(request.FirstName, request.LastName, request.DebtLimit);
            result.Id = accountId;

            return result;
        }

        public  override async Task<FloatValue> Withdraw(WithdrawRequest request, ServerCallContext context)
        {
            while (_appState.IsSystemLockForClientsOperations)
            {
                await Task.Delay(100);
            }

            FloatValue result = new FloatValue();
            var withdrawResult = await _customerService.Withdraw(request.Account, request.Ammount);
            if (withdrawResult.HasValue)
            {
                result.Value = withdrawResult.Value;
            }
            return result;
        }
        public override async Task<Empty> Deposit(DepositRequest request, ServerCallContext context)
        {
            while (_appState.IsSystemLockForClientsOperations)
            {
                await Task.Delay(100);
            }
            await _customerService.Deposit(request.Account, request.Ammount);
            return new Empty();
        }
        public override async Task<StringValue> GetHistory(UInt32Value request, ServerCallContext context)
        {
            while (_appState.IsSystemLockForClientsOperations)
            {
               await Task.Delay(100);
            }

            StringValue result = new StringValue();
            var historyResult = await _customerService.GetHistory(request.Value);
            result.Value = historyResult;

            return result;
        }
        public override async Task<BoolValue> CloseAccount(UInt32Value request, ServerCallContext context)
        {
            while (_appState.IsSystemLockForClientsOperations)
            {
                await Task.Delay(100);
            }
            BoolValue result = new BoolValue();
            var closeAccountResult = await _customerService.CloseAccount(request.Value);          
            result.Value = closeAccountResult;

            return result;
        }
      
        public override async Task<StringValue> StartInspection(Empty empty, ServerCallContext context)
        {
            StringValue result = new StringValue();
            var startInsepctionResult = await _inspectorService.StartInspection();
            result.Value = startInsepctionResult;

            return result;
        }

        public override async Task<StringValue> FinishInspection(Empty empty, ServerCallContext context)
        {
            StringValue result = new StringValue();
            var finishInspectionResult = await _inspectorService.FinishInspection();
            result.Value = finishInspectionResult;

            return result;
        }

        public override async Task GetFullSummary(Empty empty, IServerStreamWriter<StringValue> responseStream,  ServerCallContext context)
        {
            var items =  _inspectorService.GetFullSummary();
            await foreach (var item in items)
            {
                await responseStream.WriteAsync(new StringValue
                {
                    Value = $" {item}"
                });

                await Task.Delay(100);
            }
        }

    }
}