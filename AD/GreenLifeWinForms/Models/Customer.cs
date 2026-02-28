using System;

namespace GreenLifeWinForms.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegisteredDate { get; set; }

        public Customer()
        {
            IsActive = true;
            RegisteredDate = DateTime.Now;
        }
    }
}
