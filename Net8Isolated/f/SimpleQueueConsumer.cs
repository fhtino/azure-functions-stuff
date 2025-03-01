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
            _logger.LogWarning($"CONSUMER ==>>> message: {message.MessageText} - START");
            await Task.Delay(new Random().Next(200, 2000));
            _logger.LogWarning($"CONSUMER ==>>> message: {message.MessageText} - END");
        }


        [Function("SimpleQueueConsumer2")]
        public async Task Run2([QueueTrigger("simplequeue2", Connection = "DataStorage")] QueueMessage message)
        {
            _logger.LogWarning($"CONSUMER_2 ==>>> message: {message.MessageText} - START");
            await Task.Delay(new Random().Next(200, 2000));
            _logger.LogWarning($"CONSUMER_2 ==>>> message: {message.MessageText} - END");
        }

        [Function("SimpleQueueConsumer3")]
        public async Task Run3([QueueTrigger("simplequeue3", Connection = "DataStorage")] QueueMessage message)
        {
            _logger.LogWarning($"CONSUMER_3 ==>>> message: {message.MessageText} - START");
            await Task.Delay(new Random().Next(200, 2000));
            _logger.LogWarning($"CONSUMER_3 ==>>> message: {message.MessageText} - END");
        }

    }

}
