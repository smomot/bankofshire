using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

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

        public async Task GetFullSummary()
        {
            var result = _client.GetFullSummary(new Empty());

            await foreach (var reply in result.ResponseStream.ReadAllAsync())
            {
                _logger.LogTrace(reply.Value);
            }
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
