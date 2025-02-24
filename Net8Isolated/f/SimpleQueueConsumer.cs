using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Net8Isolated
{
    public class SimpleQueueConsumer
    {
        private readonly ILogger<SimpleQueueConsumer> _logger;

        public SimpleQueueConsumer(ILogger<SimpleQueueConsumer> logger)
        {
            _logger = logger;
        }


        [Function(nameof(SimpleQueueConsumer))]
        public async Task Run([QueueTrigger("simplequeue", Connection = "DataStorage")] QueueMessage message)
        {
            _logger.LogInformation($"==>>> message: {message.MessageText} - START");
            await Task.Delay(new Random().Next(200, 2000));
            _logger.LogInformation($"==>>> message: {message.MessageText} - END");
        }

    }
}
