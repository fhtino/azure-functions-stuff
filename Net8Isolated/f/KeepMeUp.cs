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
        public void Run([TimerTrigger("%KeepMeUpTimer%")] TimerInfo myTimer)
        {
            _logger.LogInformation($"KeepMeUp...");

            if (myTimer.ScheduleStatus is not null)
            {
                // NOTE: myTimer.ScheduleStatus is null when run interval is less than 1 minute
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }


        [Function("KeepMeUpAsync")]
        public async Task Run2([TimerTrigger("%KeepMeUpTimer%")] TimerInfo myTimer, CancellationToken ct)
        {
            _logger.LogInformation($"KeepMeUpAsync...");

            if (ct.IsCancellationRequested)
            {
                // do cleanup...
                return;
            }

            await Task.CompletedTask;
        } 

    }

}
