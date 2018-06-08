using System.Collections.Generic;

namespace MyWebServer.GameStore.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public decimal TotalPrice { get; set; }

        public List<OrderGame> Games { get; set; } = new List<OrderGame>();
    }
}