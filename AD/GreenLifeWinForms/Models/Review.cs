using System;

namespace GreenLifeWinForms.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        public Review()
        {
            ReviewDate = DateTime.Now;
        }
    }
}
