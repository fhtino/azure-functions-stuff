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
                [Queue("demofloodedqueue", Connection = "azstoragedemo2")] ICollector<string> outputQueueItems,
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


        [FunctionName("FloodTheQueueSync2")]
        public static async Task<IActionResult> FloodTheQueueSync2(
                [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
                [Queue("demofloodedqueue", Connection = "azstoragedemo2")] ICollector<string> outputQueueItems,
                ILogger log)
        {
            log.LogInformation("START");
            var startDT = DateTime.UtcNow;

            await Task.CompletedTask;

            int itemsCount = int.Parse(req.Query["itemsCount"]);
            if (itemsCount > 10000) throw new ApplicationException("STOP");

            var items = new List<string>();
            for (int i = 0; i < itemsCount; i++)
            {
                items.Add($"item_{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff")}_{i}");
            }

            log.LogInformation("Enqueing start");
            items.ForEach(outputQueueItems.Add);
            log.LogInformation("Enqueing end");

            log.LogInformation("END");
            return (ActionResult)new OkObjectResult($"Work done : {itemsCount} {DateTime.UtcNow.Subtract(startDT).TotalSeconds}");
        }



        [FunctionName("FloodTheQueueAsync")]
        public static async Task<IActionResult> FloodTheQueueAsync(
                [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
                [Queue("demofloodedqueue", Connection = "azstoragedemo2")] IAsyncCollector<string> outputQueueItems,
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
            [QueueTrigger("demofloodedqueueinput", Connection = "azstoragedemo2")] string inputQueueItem,
            [Queue("demofloodedqueue", Connection = "azstoragedemo2")] ICollector<string> outputQueueItems,
            ExecutionContext context,
            ILogger log)
        {
            log.LogInformation("START");
            await Task.CompletedTask;

            var items = new List<string>();
            for (int i = 0; i < 10000; i++)
            {
                items.Add($"item_{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff")}_{i}");
            }            

            log.LogInformation("Enqueing start");
            items.ForEach(outputQueueItems.Add);
            log.LogInformation("Enqueing end");
            log.LogInformation("END");
        }


        [FunctionName("FloodTheQueueAuto")]
        public static async Task FloodTheQueueAuto(
            [QueueTrigger("demofloodedqueueauto", Connection = "azstoragedemo2")] string inputQueueItem,
            [Queue("demofloodedqueueauto", Connection = "azstoragedemo2")] ICollector<string> outputQueueItems,
            ExecutionContext context,
            ILogger log)
        {
            log.LogInformation($"START : {inputQueueItem}");
            await Task.CompletedTask;

            if (inputQueueItem=="CREATE")
            {
                log.LogWarning("Creating items");

                var items = new List<string>();
                for (int i = 0; i < 1000; i++)
                {
                    items.Add($"item_{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff")}_{i}");
                }

                log.LogWarning("Enqueing start");
                items.ForEach(outputQueueItems.Add);
                log.LogWarning("Enqueing end");
            }
            else
            {
                log.LogInformation($"nothing to do");
            }

            log.LogInformation("END");
        }


    }

}

