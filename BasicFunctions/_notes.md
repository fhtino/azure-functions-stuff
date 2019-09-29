## Overview

|Function|Trigger|Binding|Notes|
|-|-|
|LongRunningTimer|Timer|-|
|LongRunningHttp|Http|-|
|QueueProducer|Http|Queue+Queue|
|QueueConsumer|Queue|-|
|JsonProducer|Http|Queue|
|InvoiceAPI|Http|-|REST API for fake invoices|



### Disable a function

Add a configuration key in the format
```
"AzureWebJobs.[function_name].Disabled": "true"
```
It works both in local.settings.json (visual studio) and on Azure.  
Ref: https://docs.microsoft.com/en-us/azure/azure-functions/disable-function

### Locked dll files during deploy
In configuration, add key MSDEPLOY_RENAME_LOCKED_FILES = 1

### Hosting
https://github.com/Azure/azure-functions-host/



### Graceful shutdown issues : experiments

On a running app-function, I deply a new version from visual studio. The app restarts. This is what happens to running fucntions.

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

Timer triggered functions are killed and cancellation token not triggered:
```
2019-08-28T00:22:20.225 [Information] i=20
2019-08-28T00:22:21.241 [Information] i=21
2019-08-28T00:22:22.241 [Information] i=22
2019-08-28T00:22:23.241 [Information] i=23
2019-08-28T00:22:23.334 [Information] Assembly reference changes detected. Restarting host...
```

On Timer triggered functions, App Insight events are lost (need to be verified on http function. Perhpas the is not related to azure function itself but is cause by the way IIS kills the app-pool).
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


