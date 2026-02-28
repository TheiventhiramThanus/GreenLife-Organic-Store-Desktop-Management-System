namespace GreenLifeWinForms.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal LineTotal { get; set; }

        public OrderItem()
        {
        }

        public OrderItem(Product product, int quantity)
        {
            ProductId = product.ProductId;
            ProductName = product.Name;
            Quantity = quantity;
            UnitPrice = product.Price;
            Discount = product.Discount;
            CalculateLineTotal();
        }

        public void CalculateLineTotal()
        {
            decimal discountedPrice = UnitPrice * (1 - Discount / 100);
            LineTotal = discountedPrice * Quantity;
        }
    }
}
