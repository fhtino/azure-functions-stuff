using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CmdBasedFunctionLIB;


namespace CmdBasedFunction
{

    public static class EnqueueMULTI
    {
        [FunctionName("EnqueueMULTI")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue(Consts.CMDQUEUENAME, Connection = "AzureWebJobsStorage")] ICollector<Cmd> outputCmds,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var runLogger = new RunLogger("EnqueueMULTI", "");

            string n = req.Query["n"];
            if (String.IsNullOrWhiteSpace(n))
                n = "1";

            outputCmds.Add(new Cmd("MULTI", "count", int.Parse(n).ToString()));

            runLogger.End(-1);

            await Task.CompletedTask;
            return (ActionResult)new OkObjectResult($"n={n}");
        }
    }

}
