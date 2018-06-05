using System;

namespace MyWebServer.ByTheCake.ViewModels.Account
{
    public class OrdersListingViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedData { get; set; }
        public decimal Sum { get; set; }
    }
}