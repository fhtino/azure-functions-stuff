using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.OData.Edm;

namespace BasicFunctions
{
    public class HighLoad
    {

        private const string QUEUENAME1 = "demohighload1";
        private const string QUEUENAME2 = "demohighload2";


        [FunctionName("HighLoadStart")]
        public static async Task<IActionResult> HighLoadStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Queue(QUEUENAME1, Connection = "azstoragedemo")] ICollector<string> outputQueueItems,
            ILogger log)
        {
            int.TryParse(req.Query["k"], out int k);

            for (int i = 0; i < k; i++)
            {
                log.LogInformation($"{i}");
                var item = $"item_{i}";
                outputQueueItems.Add(item);
            }

            return await Task.FromResult(new OkObjectResult($"OK : {k}"));
        }


        [FunctionName("HighLoadAmplify")]
        public static void HighLoadAmplify(
            [QueueTrigger(QUEUENAME1, Connection = "azstoragedemo")] string queueItem,
            [Queue(QUEUENAME2, Connection = "azstoragedemo")] ICollector<string> outputQueueItems,
            ILogger log)
        {
            log.LogInformation(queueItem);

            for (int i = 0; i < 100; i++)
            {
                outputQueueItems.Add($"{queueItem}_{i}");
            }
        }


        [FunctionName("HighLoadDoWork")]
        public static void HighLoadDoWork(
            [QueueTrigger(QUEUENAME2, Connection = "azstoragedemo")] string queueItem,
            ILogger log)
        {
            log.LogInformation($"Processing {queueItem}");
            Utility.FakeLongRunning(5);
        }


    }

}
