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

        private const string QUEUENAME = "demoqueue3";


        [FunctionName("ParamsSignature1")]
        public static async Task<IActionResult> ParamsSignature1(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("demoqueue3", Connection = "azstoragedemo")] ICollector<string> outputQueueItems1,
            [Queue(QUEUENAME, Connection = "azstoragedemo")] ICollector<string> outputQueueItems2,
            [Queue("%paramQueueName%", Connection = "azstoragedemo")] ICollector<string> outputQueueItems3,
            ILogger log)
        {
            await Task.CompletedTask;
            string message = $"NOP#lorem_ipsum#{DateTime.UtcNow.ToString("O")}";
            outputQueueItems1.Add(message);
            outputQueueItems2.Add(message);
            outputQueueItems3.Add(message);
            return new OkObjectResult("OK");
        }


        [FunctionName("ParamsSignature2")]
        public static async Task ParamsSignature2(
            [TimerTrigger("%paramTimer%")] TimerInfo myTimer,            
            ILogger log)
        {            
            await Task.CompletedTask;            
        }

    }
}
