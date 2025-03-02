using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Net8Isolated
{
    public class KeepMeUp
    {

        private readonly ILogger _logger;


        public KeepMeUp(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<KeepMeUp>();
        }


        [Function("KeepMeUp")]
        public async Task Run([TimerTrigger("%KeepMeUpTimer%")] TimerInfo myTimer, FunctionContext ctx, CancellationToken ct)
        {
            _logger.LogInformation($"{nameof(KeepMeUp)} : {ctx.FunctionDefinition.Name}");

            if (ct.IsCancellationRequested)
            {
                // do cleanup and exit
                return;
            }

            if (myTimer.ScheduleStatus is not null)
            {
                // NOTE: myTimer.ScheduleStatus is null when run interval is less than 1 minute
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

            await Task.CompletedTask;
        }

    }

}
