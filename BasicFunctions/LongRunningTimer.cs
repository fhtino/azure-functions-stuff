using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BasicFunctions
{
    public static class LongRunningTimer
    {
        [FunctionName("LongRunningTimer")]
        public static async Task Run(
            [TimerTrigger("0 0 * * * *")]TimerInfo myTimer,
            Microsoft.Azure.WebJobs.ExecutionContext context,
            CancellationToken token,
            ILogger log)
        {
            log.LogInformation($"START - InvocationId={context.InvocationId}");

            int k = 10;

            try
            {
                for (int i = 0; i < k; i++)
                {
                    log.LogInformation($"i={i}");
                    if (token.IsCancellationRequested)
                    {
                        log.LogInformation("*** CancellationToken ***");
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
        }
    }
}
