using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;


// ref:  https://markheath.net/post/azure-functions-rest-csharp-bindings
//       https://github.com/markheath/funcs-todo-csharp/blob/master/AzureFunctionsTodo/InMemory/TodoApiInMemory.cs


namespace BasicFunctions
{

    public class Invoice
    {
        public string ID { get; set; }
        public DateTime DT { get; set; }
        public string CustomerID { get; set; }
        public double Amount { get; set; }
    }


    // --> very clean API     https://developers.google.com/calendar/v3/reference/#Events
    //    https://stackoverflow.com/questions/630453/put-vs-post-in-rest/18243587#18243587


    public static class InvoiceAPI
    {

        private const string _route = "invoice";

        private static readonly List<Invoice> Items = new List<Invoice>();

        private static void CreateFakeInvoices()
        {
            Items.Clear();

            for (int i = 0; i < 10; i++)
            {
                Items.Add(
                    new Invoice
                    {
                        ID = i.ToString("00000"),
                        DT = DateTime.UtcNow,
                        CustomerID = "CUST-" + (i % 4),
                        Amount = i * 10
                    });
            }
        }


        [FunctionName("InvoiceAPI_GetAll")]
        public static async Task<IActionResult> GetAll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = _route)] HttpRequest req,
            ILogger log)
        {
            if (Items.Count == 0)
                CreateFakeInvoices();

            return new OkObjectResult(Items);
        }



        [FunctionName("InvoiceAPI_GetSingle")]   // *** GET ***
        public static async Task<IActionResult> GetSingle(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = _route + "/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {

            var item = Items.FirstOrDefault(x => x.ID == id);

            if (item != null)
                return new OkObjectResult(item);
            else
                return new NotFoundResult();
        }



        [FunctionName("InvoiceAPI_Insert")]   // *** POST ***
        public static async Task<IActionResult> Insert(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = _route)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var newInvoice = JsonConvert.DeserializeObject<Invoice>(requestBody);
            Items.Add(newInvoice);
            return new OkObjectResult(newInvoice);
        }



        [FunctionName("InvoiceAPI_Update")]  // *** PUT ***
        public static async Task<IActionResult> Update(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = _route + "/{id}")] HttpRequest req,
        string id,
        ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedInvoice = JsonConvert.DeserializeObject<Invoice>(requestBody);

            var oldInvocie = Items.SingleOrDefault(x => x.ID == updatedInvoice.ID);
            if (oldInvocie == null)
            {
                return new NotFoundResult();
            }

            Items.Remove(oldInvocie);
            Items.Add(updatedInvoice);

            return new OkResult();
        }



        // *** DELETE ***

    }
}
