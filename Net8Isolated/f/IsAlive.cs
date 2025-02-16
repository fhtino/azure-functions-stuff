using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Net8Isolated
{
    public class IsAlive
    {
        private readonly ILogger<IsAlive> _logger;

        public IsAlive(ILogger<IsAlive> logger)
        {
            _logger = logger;
        }

        [Function("isalive")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("IsAlive...");
            return new OkObjectResult(DateTime.UtcNow.ToString("O"));
        }
    }
}
