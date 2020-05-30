using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BasicFunctions
{
    public static class ParamsSignature
    {

        [FunctionName("ParamsSignature1")]
        public static async Task<IActionResult> ParamsSignature1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Queue("%paramQueueName%", Connection = "azstoragedemo")] ICollector<string> outputQueueItems,
            ILogger log)
        {
            await Task.CompletedTask;
            outputQueueItems.Add("NOP#lorem_ipsum");
            return new OkObjectResult("OK");
        }


        [FunctionName("ParamsSignature2")]
        public static async Task ParamsSignature2(
            [TimerTrigger("%paramTimer%")] TimerInfo myTimer,
            [Queue("%paramQueueName%", Connection = "azstoragedemo")] ICollector<string> outputQueueItems,
            ILogger log)
        {
            await Task.CompletedTask;
            outputQueueItems.Add("NOP#lorem_ipsum");
        }

    }
}
