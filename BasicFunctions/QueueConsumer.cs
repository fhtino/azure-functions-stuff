using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;



namespace BasicFunctions
{
    public static class QueueConsumer
    {
        [FunctionName("QueueConsumer")]
        public static async Task Run(
            [QueueTrigger("demoqueue1", Connection = "azstoragedemo")]string myQueueItem,
            Microsoft.Azure.WebJobs.ExecutionContext context,
            CancellationToken token,
            ILogger log)
        {
            log.LogWarning($"Processing: {myQueueItem}");
            await Task.Delay(100);
        }
    }
}
