using System;
using CmdBasedFunctionLIB;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CmdBasedFunction
{
    public static class EnqueueNOP
    {
        [FunctionName("EnqueueNOP")]
        public static void Run(
            [TimerTrigger("0 * * * * *")]TimerInfo myTimer,
            [Queue(Consts.CMDQUEUENAME, Connection = Consts.STORAGECONFIGKEY)]out Cmd outCmd,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            outCmd = new Cmd("NOP");
        }
    }
}
