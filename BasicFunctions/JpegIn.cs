using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;


namespace BasicFunctions
{

    public static class JpegIn
    {

        [FunctionName("JpegIn")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Request: {req.Method} {req.ContentType} {req.ContentLength}");

            byte[] imageData = null;
            using (MemoryStream ms = new MemoryStream())
            {
                await req.Body.CopyToAsync(ms);   // mandatory use of "async"
                imageData = ms.ToArray();
            }

            // TODO: store image somewhere (Blob?)            

            string b64 = Convert.ToBase64String(imageData.Take(100).ToArray());
            return new OkObjectResult($"Image Size: {imageData.Length} B64: {b64}");
        }

    }

}
