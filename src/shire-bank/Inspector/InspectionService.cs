using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;

namespace InspectorClient
{
    internal class InspectionService
    {
        private readonly ILogger<InspectionService> _logger;
        Bank.BankClient _client;

        public InspectionService(ILogger<InspectionService> logger, Bank.BankClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<string> GetFullSummary()
        {
            var result = await _client.GetFullSummaryAsync(new Empty());
            _logger.LogTrace(result.Value);
            return result.Value;
        }
        public async Task StartInspection()
        {
            _logger.LogWarning("=== Inspection started ===");
            var result = await _client.StartInspectionAsync(new Empty());
            _logger.LogWarning(result.Value);

        }
        public async Task FinishInspection()
        {
            _logger.LogWarning("=== Inspection finished  ===");
            var result = await _client.FinishInspectionAsync(new Empty());
            _logger.LogWarning(result.Value);
        }
    }
    
}
