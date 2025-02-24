using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Net8Isolated
{
    public class SimpleQueueProducer
    {
        private readonly ILogger<SimpleQueueProducer> _logger;

        public SimpleQueueProducer(ILogger<SimpleQueueProducer> logger)
        {
            _logger = logger;
        }


        [Function("SimpleQueueProducer")]
        public MyOutputType SingleItem([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation($"==>>> START");

            return new MyOutputType()
            {
                Result = new OkObjectResult("Welcome to Azure Functions!"),
                MessageText = $"Hello world {DateTime.UtcNow.ToString("O")}"
            };
        }


        public class MyOutputType
        {
            [HttpResult]
            public IActionResult Result { get; set; }

            [QueueOutput("simplequeue")]
            public string MessageText { get; set; }
        }

         
        [Function("SimpleQueueProducerMulti")]
        public MyOutputTypeMultiItems MultiItems([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation($"==>>> START");

            return new MyOutputTypeMultiItems()
            {
                Result = new OkObjectResult("Welcome to Azure Functions!"),
                MessageText = Enumerable.Range(0, 10).Select(i => $"{i} : Hello world {DateTime.UtcNow.ToString("O")}").ToList()
            };
        }


        public class MyOutputTypeMultiItems
        {
            [HttpResult]
            public IActionResult Result { get; set; }

            [QueueOutput("simplequeue", Connection = "DataStorage")]
            public List<string> MessageText { get; set; }
        }

    }
}
