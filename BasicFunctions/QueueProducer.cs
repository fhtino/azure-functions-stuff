using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;


namespace BasicFunctions
{
    public static class QueueProducer
    {
        [FunctionName("QueueProducer")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Queue("demoqueue1", Connection = "azstoragedemo")] ICollector<string> outputQueueItems,
            [Queue("demoqueue2", Connection = "azstoragedemo")] ICollector<string> outputQueueItems2,
            Microsoft.Azure.WebJobs.ExecutionContext context,
            CancellationToken token,
            ILogger log)
        {
            // Documentation: https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue?tabs=csharp

            log.LogInformation("START");

            int.TryParse(req.Query["k"], out int k);

            log.LogInformation($"k={k}");

            for (int i = 0; i < k; i++)
            {
                // Note: items are immediatelly added to the output queue and not 
                //       temporary stored in outputQueueItems and all added at the end.
                outputQueueItems.Add("NOP#1#" + k);
                outputQueueItems2.Add("NOP#2#" + k);
                await Task.Delay(500);
            }

            log.LogInformation("END");
            return (ActionResult)new OkObjectResult($"All done");
        }
    }
}
