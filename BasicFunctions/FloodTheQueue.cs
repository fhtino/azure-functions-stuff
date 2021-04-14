using System;
using System.Collections.Generic;
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

        [FunctionName("FloodTheQueueSync")]
        public static async Task<IActionResult> FloodTheQueueSync(
                [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
                [Queue("demofloodedqueue", Connection = "azstoragedemo")] ICollector<string> outputQueueItems,
                ILogger log)
        {
            log.LogInformation("START");
            var startDT = DateTime.UtcNow;

            await Task.CompletedTask;

            int itemsCount = int.Parse(req.Query["itemsCount"]);
            if (itemsCount > 10000) throw new ApplicationException("STOP");

            log.LogInformation("Enqueing start");
            for (int i = 0; i < itemsCount; i++)
            {
                outputQueueItems.Add($"item_{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff")}_{i}");
            }
            log.LogInformation("Enqueing end");

            log.LogInformation("END");
            return (ActionResult)new OkObjectResult($"Work done : {itemsCount} {DateTime.UtcNow.Subtract(startDT).TotalSeconds}");
        }



        [FunctionName("FloodTheQueueAsync")]
        public static async Task<IActionResult> FloodTheQueueAsync(
                [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
                [Queue("demofloodedqueue", Connection = "azstoragedemo")] IAsyncCollector<string> outputQueueItems,
                ILogger log)
        {
            log.LogInformation("START");
            var startDT = DateTime.UtcNow;

            await Task.CompletedTask;

            int itemsCount = int.Parse(req.Query["itemsCount"]);
            if (itemsCount > 10000) throw new ApplicationException("STOP");

            log.LogInformation("Enqueing start");
            for (int i = 0; i < itemsCount; i++)
            {
                await outputQueueItems.AddAsync($"item_{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff")}_{i}");
            }
            log.LogInformation("Enqueing end");

            log.LogInformation("END");
            return (ActionResult)new OkObjectResult($"Work done : {itemsCount} {DateTime.UtcNow.Subtract(startDT).TotalSeconds}");
        }


        [FunctionName("FloodTheQueueFromQueue")]
        public static async Task FloodTheQueueFromQueue(
            [QueueTrigger("demofloodedqueueinput", Connection = "azstoragedemo")] string inputQueueItem,
            [Queue("demofloodedqueue", Connection = "azstoragedemo")] ICollector<string> outputQueueItems,
            ExecutionContext context,
            ILogger log)
        {
            log.LogInformation("START");
            await Task.CompletedTask;
            for (int i = 0; i < 1000; i++)
            {
                outputQueueItems.Add($"item_{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff")}_{i}");
            }
            log.LogInformation("END");
        }

    }

}

