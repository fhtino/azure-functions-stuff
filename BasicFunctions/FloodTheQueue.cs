using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BasicFunctions
{

    public static class FloodTheQueue
    {

        [FunctionName("FloodTheQueue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Queue("demofloodedqueue", Connection = "azstoragedemo")] ICollector<string> outputQueueItems,
            ILogger log)
        {
            log.LogInformation("START");

            int itemsCount = int.Parse(req.Query["itemsCount"]);

            if (itemsCount > 10000) throw new ApplicationException("STOP");


            for (int i = 0; i < itemsCount; i++)
            {
                outputQueueItems.Add($"item_{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff")}_{i}");
            }



            log.LogInformation("END");
            return (ActionResult)new OkObjectResult($"Work done");
        }

    }

}

