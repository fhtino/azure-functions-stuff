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
using System.Web.Http;

namespace BasicFunctions
{

    public static class LongRunningHttp
    {

        [FunctionName("LongRunningHttp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            Microsoft.Azure.WebJobs.ExecutionContext context,
            CancellationToken token,
            ILogger log)
        {

            log.LogInformation($"START - InvocationId={context.InvocationId} - CC");

            int.TryParse(req.Query["k"], out int k);
            if (k == 0) k = 5;
            if (k > 60) k = 60;
            
            try
            {
                // fake long running
                for (int i = 0; i < k; i++)
                {
                    log.LogInformation($"i={i}");
                    if (token.IsCancellationRequested)
                    {
                        log.LogInformation("*** CancellationToken ***");
                        // if need to exit with error: return new ExceptionResult(new ApplicationException("STOP"), false);
                    }
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message);
                log.LogCritical(ex, ex.ToString());
            }

            log.LogInformation($"END");

            return (ActionResult)new OkObjectResult($"Hello, world!  k={k}");
        }
    }

}
