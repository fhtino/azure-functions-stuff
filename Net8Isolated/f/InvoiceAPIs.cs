using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace Net8Isolated
{

    public class Invoice
    {
        public string ID { get; set; }
        public DateTime DT { get; set; }
        public string CustomerID { get; set; }
        public double Amount { get; set; }
        public DateTime UpdateDT { get; set; }
    }


    public class InvoiceAPIs
    {
        private const string _route = "invoice";

        private readonly ILogger<InvoiceAPIs> _logger;

        public InvoiceAPIs(ILogger<InvoiceAPIs> logger)
        {
            _logger = logger;
        }


        // This only works for demo purposes. 
        private static readonly List<Invoice> InvoiceList = new List<Invoice>();


        private void CreateFakeInvoices()
        {
            InvoiceList.Clear();

            for (int i = 0; i < 10; i++)
            {
                InvoiceList.Add(
                    new Invoice
                    {
                        ID = i.ToString("00000"),
                        DT = DateTime.UtcNow,
                        CustomerID = "CUST-" + (i % 4),
                        Amount = i * 10,
                        UpdateDT = DateTime.UtcNow
                    });
            }
        }


        [Function("InvoiceAPIs_GetAll")]   // *** GET ***
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = _route)] HttpRequest req, FunctionContext ctx)
        {
            _logger.LogInformation($"Function: {ctx.FunctionDefinition.Name}");

            if (InvoiceList.Count == 0) CreateFakeInvoices();
            await Task.CompletedTask;
            return new OkObjectResult(InvoiceList);
        }


        [Function("InvoiceAPI_GetSingle")]   // *** GET ***
        public async Task<IActionResult> GetSingle([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = _route + "/{id}")] HttpRequest req, string id)
        {
            var item = InvoiceList.FirstOrDefault(x => x.ID == id);
            await Task.CompletedTask;
            return (item != null) ? new OkObjectResult(item) : new NotFoundResult();
        }


        [Function("InvoiceAPI_Insert")]   // *** POST ***
        public async Task<IActionResult> Insert([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = _route)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var newInvoice = JsonSerializer.Deserialize<Invoice>(requestBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            newInvoice.UpdateDT = DateTime.UtcNow;
            if (newInvoice == null) return new BadRequestResult();
            InvoiceList.Add(newInvoice);
            return new OkObjectResult(newInvoice);
        }

        [Function("InvoiceAPI_Update")]  // *** PUT ***
        public async Task<IActionResult> Update([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = _route)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var incomingInvoice = JsonSerializer.Deserialize<Invoice>(requestBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            incomingInvoice.UpdateDT = DateTime.UtcNow;
            var oldInvocie = InvoiceList.SingleOrDefault(x => x.ID == incomingInvoice.ID);
            if (oldInvocie == null) { return new NotFoundResult(); }
            InvoiceList.Remove(oldInvocie);
            InvoiceList.Add(incomingInvoice);
            return new OkObjectResult(incomingInvoice);
        }


        [Function("InvoiceAPI_Delete")]   // *** DELETE ***
        public async Task<IActionResult> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = _route + "/{id}")] HttpRequest req, string id)
        {
            var item = InvoiceList.FirstOrDefault(x => x.ID == id);
            if (item == null) return new NotFoundResult();
            await Task.CompletedTask;
            InvoiceList.Remove(item);
            return new OkResult();
        }

    }

}
