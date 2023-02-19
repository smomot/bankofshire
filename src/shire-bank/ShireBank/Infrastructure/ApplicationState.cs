

namespace ShireBankService.Infrastructure
{
    public class ApplicationState
    {
        readonly ILogger<ApplicationState> _logger;
        public ApplicationState(ILogger<ApplicationState> logger)
        {
            _logger = logger;
        }
        public bool IsSystemLockForClientsOperations { get; private set; }

        public string LockSystemForClientOperations()
        {
            IsSystemLockForClientsOperations = true;
            var result = "### SYSTEM LOCKED FOR USERS OPERATION - ALL HOBBIT OPERATIONS SUSPENDED ###";
            _logger.LogWarning(result);
            return result;
        }


        public string UnlockSystemForClientOperations()
        {
            IsSystemLockForClientsOperations = false;
            var result = "### SYSTEM UNLOCKED FOR USERS OPERATION - ALL HOBBIT OPERATIONS RESEUMED ###";
            _logger.LogWarning(result);
            return result;

        }
    }
}
