using System;

namespace GreenLifeWinForms.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Supplier { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal Discount { get; set; }
        public bool IsActive { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        // Calculated properties
        public decimal DiscountedPrice
        {
            get
            {
                if (Discount > 0)
                {
                    return Price * (1 - Discount / 100);
                }
                return Price;
            }
        }

        public bool IsLowStock
        {
            get { return Stock < 50; }
        }

        public string StockStatus
        {
            get
            {
                if (Stock < 50) return "Low Stock";
                if (Stock < 100) return "Medium Stock";
                return "Good Stock";
            }
        }

        // For display purposes
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
