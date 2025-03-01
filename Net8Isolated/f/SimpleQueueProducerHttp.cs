using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;


namespace Net8Isolated
{

    public class SimpleQueueProducerHttp
    {

        private readonly ILogger<SimpleQueueProducerHttp> _logger;

        public SimpleQueueProducerHttp(ILogger<SimpleQueueProducerHttp> logger)
        {
            _logger = logger;
        }

        // ---------------------------------------------------------------------------------------------------------------------------

        [Function("SimpleQueueProducerHttp")]
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

        // ---------------------------------------------------------------------------------------------------------------------------
        // 
        [Function("SimpleQueueProducerHttpMulti")]
        public MyOutputTypeMultiItems MultiItems([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation($"==>>> START");

            return new MyOutputTypeMultiItems()
            {
                Result = new OkObjectResult("Welcome to Azure Functions!"),
                MessageList = Enumerable.Range(0, 10).Select(i => $"{i} : Hello world {DateTime.UtcNow.ToString("O")}").ToList()
            };
        }


        public class MyOutputTypeMultiItems
        {
            [HttpResult]
            public IActionResult Result { get; set; }

            [QueueOutput("simplequeue", Connection = "DataStorage")]
            public List<string> MessageList { get; set; }
        }

        // ---------------------------------------------------------------------------------------------------------------------------

        [Function("SimpleQueueProducerHttpMultiQueue")]
        public OutputMultiItemsAndQueues MultiItemsMultiQueues([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, FunctionContext ctx)
        {
            _logger.LogWarning($"{ctx.FunctionDefinition.Name} ==>>> START");

            return new OutputMultiItemsAndQueues()
            {
                Result = new OkObjectResult("Welcome to Azure Functions!"),
                Queue1MessageList = Enumerable.Range(0, 1).Select(i => $"{i} : Hello world queue1 {DateTime.UtcNow.ToString("O")}").ToList(),
                Queue2MessageList = Enumerable.Range(0, 1).Select(i => $"{i} : Hello world queue2 {DateTime.UtcNow.ToString("O")}").ToList(),
                Queue3MessageList = Enumerable.Range(0, 2).Select(i => $"{i} : Hello world queue3 {DateTime.UtcNow.ToString("O")}").ToList(),
            };
        }


        public class OutputMultiItemsAndQueues
        {
            [HttpResult]
            public IActionResult Result { get; set; }

            [QueueOutput("simplequeue", Connection = "DataStorage")]
            public List<string> Queue1MessageList { get; set; }

            [QueueOutput("simplequeue2", Connection = "DataStorage")]
            public List<string> Queue2MessageList { get; set; }

            [QueueOutput("simplequeue3", Connection = "DataStorage")]
            public List<string> Queue3MessageList { get; set; }
        }

        // ---------------------------------------------------------------------------------------------------------------------------

    }
}
