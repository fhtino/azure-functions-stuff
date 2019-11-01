using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;

namespace BasicFunctions
{

    public class CustomMetrics
    {
        // About Application Insights custome metrics: https://docs.microsoft.com/en-us/azure/azure-monitor/app/api-custom-events-metrics

        private readonly TelemetryClient telemetryClient;

        public CustomMetrics(TelemetryConfiguration telemetryConfiguration)
        {
            this.telemetryClient = new TelemetryClient(telemetryConfiguration);
        }


        [FunctionName("CustomMetrics")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Function start");

            telemetryClient.TrackEvent("my message");
            telemetryClient.TrackTrace("my other message", SeverityLevel.Error);
            telemetryClient.GetMetric("mymetricx").TrackValue(DateTime.UtcNow.Millisecond);            

            return (ActionResult)new OkObjectResult($"Hello, world");
        }

    }
}
