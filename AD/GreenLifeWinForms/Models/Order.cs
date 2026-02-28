using System;
using System.Collections.Generic;

namespace GreenLifeWinForms.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // Pending, Shipped, Delivered
        public decimal Subtotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal GrandTotal { get; set; }

        // Navigation property
        public List<OrderItem> Items { get; set; }

        public Order()
        {
            OrderDate = DateTime.Now;
            Status = "Pending";
            Items = new List<OrderItem>();
        }
    }
}
