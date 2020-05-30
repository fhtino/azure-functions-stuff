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
using BasicFunctions.Models;

namespace BasicFunctions
{
    public class FoodDemo
    {

        [FunctionName("FoodDemo_Products")]
        public async Task<IActionResult> ProductsGet([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "fooddemo/products")] HttpRequest req, ILogger log)
        {
            var products = FakeDB.GetData();
            await Task.CompletedTask;
            return new OkObjectResult(products);
        }


        [FunctionName("FoodDemo_Cart")]
        public async Task<IActionResult> CartPost([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "fooddemo/cart")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var products = JsonConvert.DeserializeObject<Product[]>(requestBody);

            // create an order
            var order = new Order();
            order.OrderID = Guid.NewGuid().ToString().ToUpper().Substring(0, 8);
            order.TotalAmount = products.Sum(x => x.Price);
            order.Items = products;
            order.DT = DateTime.UtcNow;
            order.DeliveryDate = DateTime.UtcNow.AddDays(new Random().NextDouble());

            return new OkObjectResult(order);
        }

    }



    public class FakeDB
    {
        public static Product[] GetData()
        {
            var loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque non mi vel ex interdum placerat. Sed fringilla porta ante nec venenatis.";

            var products = new List<Product>();
            products.Add(new Product() { ID = "P56236", Description = "Vegetable Fried Rice", LongDescription = loremIpsum, Price = 4.75 });
            products.Add(new Product() { ID = "P56237", Description = "Tod Mun Pla", LongDescription = loremIpsum, Price = 8.25 });
            products.Add(new Product() { ID = "P56238", Description = "Noodle ", LongDescription = loremIpsum, Price = 5.00 });
            products.Add(new Product() { ID = "P56239", Description = "Dolsot bibimbap", LongDescription = loremIpsum, Price = 7.80 });
            products.Add(new Product() { ID = "P56240", Description = "Thai Green Curry", LongDescription = loremIpsum, Price = 5.80 });
            products.Add(new Product() { ID = "P56241", Description = "Thai Mok", LongDescription = loremIpsum, Price = 6.30 });
            products.Add(new Product() { ID = "P56242", Description = "Ramen Bowl", LongDescription = loremIpsum, Price = 9.40 });

            return products.ToArray();
        }
    }

}
