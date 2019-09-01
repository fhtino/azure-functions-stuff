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
    public static class JsonProducer
    {
        [FunctionName("JsonProducer")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Queue("demoqueue3", Connection = "azstoragedemo")] IAsyncCollector<Person> outputQueueItems,
            Microsoft.Azure.WebJobs.ExecutionContext context,
            CancellationToken token,
            ILogger log)
        {
            log.LogInformation("Start...");

            var p = new Person
            {
                ID = (DateTime.UtcNow.Ticks % 100000).ToString(),
                Name = "John Doe",
                Address = "Fake address",
                BirthDate = DateTime.UtcNow,
                Weight = 65.1234
            };

            await outputQueueItems.AddAsync(p);

            return (ActionResult)new OkObjectResult($"all done");
        }
    }
}
