
# Notes


## My takeay
 
 ...

## Local Development (settings)

local.settings.json

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "KeepMeUpTimer": "0 */3 * * * *"
  }
}
```

## Notes

 - Do not use static methods.

## Queue producers and cusumers

```mermaid
graph LR 
  Queue1@{ shape: das }
  Queue2@{ shape: das }
  Queue3@{ shape: das }
  Queue1 --> QueueConsumer1
  Queue2 --> QueueConsumer2
  Queue3 --> QueueConsumer3
  Producer_TIMER@{ shape: hex } --> Queue1
  Producer_HTTP --> Queue1
  ProducerMulti_HTTP --> Queue1
  ProducerMultiQueue_HTTP --> Queue1 & Queue2 & Queue3
```

## Queue high load

```mermaid
graph LR 
  QueueM1@{ shape: das }
  QueueM2@{ shape: das }
  Generator_http --> QueueM1
  QueueM1 --> Multiply[Multiply x10] --> QueueM2
  QueueM2 --> Consumer
```

[. . . . TODO . . . .]

## Mermeid info:

```mermaid
  info
```

## Links

HTTP trigger



Details here:  https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide