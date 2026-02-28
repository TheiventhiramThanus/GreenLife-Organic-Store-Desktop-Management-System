using System;

namespace GreenLifeWinForms.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }

        public Admin()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
