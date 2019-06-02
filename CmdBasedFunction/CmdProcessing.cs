using System;
using System.Threading.Tasks;
using CmdBasedFunctionLIB;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;


namespace CmdBasedFunction
{
    public static class CmdProcessing
    {

        [FunctionName("CmdProcessing")]
        public static async Task Run(
            [QueueTrigger(Consts.CMDQUEUENAME, Connection = "AzureWebJobsStorage")] string myQueueItem,
            [Queue(Consts.CMDQUEUENAME, Connection = "AzureWebJobsStorage")] ICollector<string> outputQueueItems,
            ILogger log)
        {
            log.LogInformation($"CmdProcessing : {myQueueItem}");

            var inputCmd = JsonConvert.DeserializeObject<Cmd>(myQueueItem);
            var outputCmdList = await CmdProcessor.Execute(inputCmd, log);

            if (outputCmdList != null)
            {
                outputCmdList
                    .Select(x => JsonConvert.SerializeObject(x))
                    .ToList()
                    .ForEach(outputQueueItems.Add);
            }
        }
    }
}
