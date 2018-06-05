using System;

namespace MyWebServer.ByTheCake.ViewModels.Account
{
    public class ProfileVewModel
    {
        public string Username { get; set; }
        public DateTime RegitrationDate { get; set; }
        public int TotalOrders { get; set; }
    }
}