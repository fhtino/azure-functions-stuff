using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Net8Isolated
{
    public class QueueConsumer
    {
        private readonly ILogger<QueueConsumer> _logger;

        public QueueConsumer(ILogger<QueueConsumer> logger)
        {
            _logger = logger;
        }


        [Function("QueueConsumer1")]
        public async Task Run([QueueTrigger("queue1", Connection = "DataStorage")] QueueMessage message)
        {
            _logger.LogWarning($"CONSUMER_1 ==>>> message: {message.MessageText} - START");
            await Task.Delay(new Random().Next(200, 2000));
            _logger.LogWarning($"CONSUMER_1 ==>>> message: {message.MessageText} - END");
        }


        [Function("QueueConsumer2")]
        public async Task Run2([QueueTrigger("queue2", Connection = "DataStorage")] QueueMessage message)
        {
            _logger.LogWarning($"CONSUMER_2 ==>>> message: {message.MessageText} - START");
            await Task.Delay(new Random().Next(200, 2000));
            _logger.LogWarning($"CONSUMER_2 ==>>> message: {message.MessageText} - END");
        }


        [Function("QueueConsumer3")]
        public async Task Run3([QueueTrigger("queue3", Connection = "DataStorage")] QueueMessage message)
        {
            _logger.LogWarning($"CONSUMER_3 ==>>> message: {message.MessageText} - START");
            await Task.Delay(new Random().Next(200, 2000));
            _logger.LogWarning($"CONSUMER_3 ==>>> message: {message.MessageText} - END");
        }

    }

}
