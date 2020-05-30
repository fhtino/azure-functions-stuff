# Notes...

## Overview

|Function|Trigger|Binding|Notes|
|-|-|
|LongRunningTimer|Timer|-|
|LongRunningHttp|Http|-|
|QueueProducer|Http|Queue+Queue|
|QueueConsumer|Queue|-|
|JsonProducer|Http|Queue|
|InvoiceAPI|Http|-|REST API for fake invoices|
|CustomMetrics|Http|-|Application Insights traces and custom metrics|
|reCap | Http | - | reCaptcha in Azure Functions (html output) |
|FoodDemo | Http | - | Food demo |


### Configuration file
Before running, add a local.settings.json file with required settings.  
Example:
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "AzureWebJobs.LongRunningTimer.Disabled": "true",
    "AzureWebJobs.QueueConsumer.Disabled": "false",
    "azstoragedemo": "UseDevelopmentStorage=true",
    "APPINSIGHTS_INSTRUMENTATIONKEY": "xxxxxxxxx",
    "reCaptchaPrivateKey": "rrrrrrrrr"
  }
}
```

### FunctionsStartup 
Steps:
 - Add reference to:
   - Microsoft.Azure.Functions.Extensions
   - Microsoft.NET.Sdk.Functions  >= 1.0.28
 - Create a Startup class, extending FunctionsStartup and overriding Configure method
 - Mark the assembly with FunctionsStartup attribute

```csharp
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BasicFunctions.Startup))]

namespace BasicFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // code...
        }
    }
}
```

Details: https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection

### CORS
In local.settings.json
```json
  "Host": {
    "CORS": "*"
  }
```

### Disable a function

Add a configuration key in the format
```
"AzureWebJobs.[function_name].Disabled": "true"
```
It works both in local.settings.json (visual studio) and on Azure.  
Ref: https://docs.microsoft.com/en-us/azure/azure-functions/disable-function

### Locked dll files during deploy
In configuration, add key MSDEPLOY_RENAME_LOCKED_FILES = 1

### Application Insights custom data (CustomMetrics)
Azure Functions automatically sends data to Application Insights if APPINSIGHTS_INSTRUMENTATIONKEY is present.
To manually use TelemetryClient from code, e.g. for sending custom Trace or Metric, it's required to add a reference to App Insight Nuget packages.  
Do not add "low-level" packages directly but, instead, add only Microsoft.Azure.WebJobs.Logging.ApplicationInsights It will bring all required packages in a compatible way to the hosting environment. More details here: https://github.com/MicrosoftDocs/azure-docs/issues/35181#issuecomment-512288993  
If Nuget packages are not compatible **OR** the key APPINSIGHTS_INSTRUMENTATIONKEY is missing, you'll get errors like:  
``` 
Microsoft.Extensions.DependencyInjection.Abstractions: Unable to resolve service  
for type 'Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration'   
while attempting to activate 'BasicFunctions.CustomMetrics'
``` 
More information about App Insights in Functions:
 - https://docs.microsoft.com/en-us/azure/azure-monitor/app/azure-functions-supported-features  (a bit outdated)  
 - https://docs.microsoft.com/en-us/azure/azure-functions/functions-monitoring#log-custom-telemetry-in-c-functions    
 
**Note:** functions must be declared as not static and a constructor is required to get the pre-configured TelemetryClient.

### Hosting
https://github.com/Azure/azure-functions-host/


## Misc

### Graceful shutdown issues : experiments

On a running app-function, I deploy a new version from visual studio. The app restarts. This is what happens to running fucntions.

Http triggered fucntions seem to honor the cancellationn token. Timeout around 5 seconds.

```
2019-08-28T00:42:24.298 [Information] i=25
2019-08-28T00:42:25.330 [Information] i=26
2019-08-28T00:42:26.367 [Information] i=27
2019-08-28T00:42:27.128 [Information] Assembly reference changes detected. Restarting host...
2019-08-28T00:42:27.376 [Information] i=28
2019-08-28T00:42:27.377 [Information] *** CancellationToken ***
2019-08-28T00:42:28.384 [Information] i=29
2019-08-28T00:42:28.385 [Information] *** CancellationToken ***
2019-08-28T00:42:29.401 [Information] i=30
2019-08-28T00:42:29.401 [Information] *** CancellationToken ***
2019-08-28T00:42:30.407 [Information] i=31
2019-08-28T00:42:30.408 [Information] *** CancellationToken ***
2019-08-28T00:42:31.423 [Information] i=32
2019-08-28T00:42:31.424 [Information] *** CancellationToken ***
2019-08-28T00:43:52  No new trace in the past 1 min(s).
```

Timer triggered functions have a different behaviour compared to Http triggered. The shutdown process is faster and checking the token with "if (token.IsCancellationRequested)" is not working as expected. Instead, regiter to the event with:
token.Register(() => { \... });

```
[01/06/2020 08:17:40] i=0
[01/06/2020 08:17:41] i=1
[01/06/2020 08:17:42] i=2
[01/06/2020 08:17:43] i=3
[01/06/2020 08:17:44] i=4
[01/06/2020 08:17:44] *** CancellationToken ***
[01/06/2020 08:17:45] i=5
[01/06/2020 08:17:46] i=6
[01/06/2020 08:17:47] i=7
[01/06/2020 08:17:48] i=8
[01/06/2020 08:17:49] i=9
[01/06/2020 08:17:49] Host did not shutdown within its allotted time.
```

On Timer triggered functions, during shutwodn, App Insight events are lost (need to be verified on http function. Perhpas the is not related to azure function itself but is cause by the way IIS kills the app-pool).
Kusto query to get items from Appplication Insights.

```
traces 
| where timestamp > ago(100m)
| where customDimensions["InvocationId"] == "b6f2xxxxx"
| take (1000)
| order by timestamp desc
```


#### links
https://github.com/Azure/azure-functions-host/issues/2153   <<<===  
https://github.com/Azure/azure-functions-host/issues/4251  
https://github.com/Azure/Azure-Functions/issues/866  
https://github.com/Azure/Azure-Functions/issues/862  


### TODO

Authentication  

https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook#secure-an-http-endpoint-in-production


https://blogs.msdn.microsoft.com/stuartleeks/2018/02/19/azure-functions-and-app-service-authentication/
https://stackoverflow.com/questions/51413576/azure-functions-authentication

https://docs.microsoft.com/en-us/azure/app-service/overview-authentication-authorization






EasyAuth not working ???

https://github.com/Azure/azure-functions-host/issues/33
https://github.com/Azure/azure-functions-host/issues/3898


   var xxxx = req.HttpContext.User.Identity.IsAuthenticated;
            //https://<my azure functions app url>/.auth/me


