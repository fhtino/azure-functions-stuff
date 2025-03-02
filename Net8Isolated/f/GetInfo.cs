using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Net8Isolated.f
{
    public class GetInfo
    {
        private readonly ILogger<GetInfo> _logger;

        public GetInfo(ILogger<GetInfo> logger)
        {
            _logger = logger;
        }

        [Function("GetInfo")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, FunctionContext ctx)
        {           
            var outList = new List<(string, string?)>();

            

            var cp = System.Diagnostics.Process.GetCurrentProcess();

            outList.Add(("ProcessName", cp.ProcessName));
            outList.Add(("MainModule.FileName", cp.MainModule?.FileName));
            outList.Add(("Environment.CommandLine", Environment.CommandLine));

            // outList.Add(("", ));


            

            outList.Add(("FunctionDefinition.Id", ctx.FunctionDefinition.Id));
            outList.Add(("ctx.FunctionDefinition.Name", ctx.FunctionDefinition.Name));


            //ctx.BindingContext.BindingData.ToList().ForEach(x=>x.v)

            return new OkObjectResult(String.Join("\n\n", outList.Select(x => $"{x.Item1} = {x.Item2} ")));
        }
    }
}
