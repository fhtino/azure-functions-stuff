using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;


namespace Net8Isolated
{

    public class QueueProducers
    {

        private readonly ILogger<QueueProducers> _logger;

        public QueueProducers(ILogger<QueueProducers> logger)
        {
            _logger = logger;
        }


        // ---------------------------------------------------------------------------------------------------------------------------


        [Function("QueueProducerTimer")]
        [QueueOutput("queue1")]
        public async Task<List<string>> Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer)
        {
            await Task.CompletedTask;
            var outList = Enumerable.Range(0, 2).Select(i => $"HelloWorld from QueueProducerTimer : {i} : {DateTime.Now}").ToList();
            _logger.LogWarning($"QueueProducerTimer : {outList.Count()}");
            return outList;
        }


        // ---------------------------------------------------------------------------------------------------------------------------

        [Function("QueueProducerHttp")]
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

            [QueueOutput("queue1")]
            public string MessageText { get; set; }
        }


        // ---------------------------------------------------------------------------------------------------------------------------
        
        [Function("QueueProducerHttpMulti")]
        public OutputMultiItems MultiItems([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation($"==>>> START");

            return new OutputMultiItems()
            {
                Result = new OkObjectResult("Welcome to Azure Functions!"),
                MessageList = Enumerable.Range(0, 10).Select(i => $"{i} : Hello world {DateTime.UtcNow.ToString("O")}").ToList()
            };
        }

        public class OutputMultiItems
        {
            [HttpResult]
            public IActionResult Result { get; set; }

            [QueueOutput("queue1", Connection = "DataStorage")]
            public List<string> MessageList { get; set; }
        }

        // ---------------------------------------------------------------------------------------------------------------------------

        [Function("QueueProducerHttpMultiQueue")]
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

            [QueueOutput("queue1", Connection = "DataStorage")]
            public List<string> Queue1MessageList { get; set; }

            [QueueOutput("queue2", Connection = "DataStorage")]
            public List<string> Queue2MessageList { get; set; }

            [QueueOutput("queue3", Connection = "DataStorage")]
            public List<string> Queue3MessageList { get; set; }
        }

        // ---------------------------------------------------------------------------------------------------------------------------

    }
}
