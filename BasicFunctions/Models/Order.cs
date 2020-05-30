using System;
using System.Collections.Generic;
using System.Text;


namespace BasicFunctions.Models
{
    public class Order
    {
        public string OrderID { get; set; }
        public double TotalAmount { get; set; }
        public DateTime DT { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Product[] Items { get; set; }
    }
}
