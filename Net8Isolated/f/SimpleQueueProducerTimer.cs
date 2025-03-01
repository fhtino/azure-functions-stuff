using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Net8Isolated
{
    public class SimpleQueueProducerTimer
    {
        private readonly ILogger _logger;

        public SimpleQueueProducerTimer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SimpleQueueProducerTimer>();
        }


        [Function("SimpleQueueProducerTimer")]
        [QueueOutput("simplequeue")]
        public async Task<List<string>> Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer)
        {
            await Task.CompletedTask;
            var outList = Enumerable.Range(0, 2).Select(i => $"HelloWorld from {nameof(SimpleQueueProducerTimer)} : {i} : {DateTime.Now}").ToList();            
            _logger.LogWarning($"{nameof(SimpleQueueProducerTimer)} : {outList.Count()}");
            return outList;
        }

    }
}
