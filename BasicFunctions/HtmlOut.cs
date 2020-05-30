using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;  //<---
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BasicFunctions
{
    public static class HtmlOut
    {
        [FunctionName("HtmlOut")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext execContext)
        {
            string html;
            var mode = req.Query["mode"];

            switch (mode)
            {
                case "code":
                    html = "<html><body><h1>Hello, world</h1></body></html>";
                    break;

                case "file1":
                    var hosting = req.HttpContext.RequestServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
                    html = File.ReadAllText(Path.Combine(hosting.ContentRootPath, "Data/page1.html"));
                    break;

                case "file2":
                    // ExecutionContext - https://github.com/Azure/azure-functions-host/wiki/Retrieving-information-about-the-currently-running-function
                    html = File.ReadAllText(Path.Combine(execContext.FunctionAppDirectory, "Data/page1.html"));
                    break;

                default:
                    html = "default";
                    break;
            }

            await Task.CompletedTask;

            return
                new ContentResult()
                {
                    Content = html,
                    ContentType = "text/html",
                    StatusCode = 200
                };
        }
    }
}
