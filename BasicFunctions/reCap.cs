using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;


namespace BasicFunctions
{

    public static class reCap
    {

        private static HttpClient _httpClient = new HttpClient();


        [FunctionName("reCap")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext execContext)
        {
            log.LogInformation("Start");

            string html;

            if (req.Method == "GET")
            {
                html = File.ReadAllText(Path.Combine(execContext.FunctionAppDirectory, "reCap1.html"));
            }
            else if (req.Method == "POST")
            {
                string token = req.Form["reCapToken"];
                bool resultOK = await ReCaptchaPassed(token);
                html = File.ReadAllText(Path.Combine(execContext.FunctionAppDirectory, "reCap2.html"));
                html = html.Replace("###RESULT###", resultOK ? "OK" : "ERROR");
            }
            else
            {
                html = "<html><body>Not supported</body></html>";
            }

            return
              new ContentResult()
              {
                  Content = html,
                  ContentType = "text/html",
                  StatusCode = 200
              };
        }


        public static async Task<bool> ReCaptchaPassed(string gRecaptchaResponse)
        {
            var reCaptchaPrivateKey = Environment.GetEnvironmentVariable("reCaptchaPrivateKey");

            var res = await _httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={reCaptchaPrivateKey}&response={gRecaptchaResponse}");
            if (res.StatusCode != HttpStatusCode.OK)
                return false;

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = Newtonsoft.Json.Linq.JObject.Parse(JSONres);
            return JSONdata.success == "true";
        }
    }
}
