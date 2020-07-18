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
    public static class HelloWorld
    {

        [FunctionName("HelloWorld1")]
        public static async Task<IActionResult> HelloWorld1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("start function");
            string name = req.Query["name"];
            string responseMessage = $"Hello, {name}";
            return new OkObjectResult(responseMessage);
        }



        [FunctionName("HelloWorld2")]
        public static async Task<IActionResult> HelloWorld2(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
      ILogger log)
        {
            log.LogInformation("start function");
            string name = req.Query["name"];
            var person = new { Name = name, ID = Guid.NewGuid().ToString(), DT = DateTime.UtcNow };

            // In my experiments, content-type is always json if I use anonymous type.
            // With real types, the content negotiaton of OkObjectResult works as expected.
            return new OkObjectResult(person);
        }


        [FunctionName("HelloWorld999")]
        public static async Task<IActionResult> HelloWorld999(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", "delete", Route = null)] HttpRequest req,
    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

    }
}
