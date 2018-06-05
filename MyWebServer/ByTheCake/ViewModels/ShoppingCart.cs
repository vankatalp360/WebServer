using System.Collections.Generic;
using MyWebServer.ByTheCake.Data.Models;

namespace MyWebServer.ByTheCake.ViewModels
{
    public class ShoppingCart
    {
        public const string SessionKey = "%^Current_Shopping_Cart^%";

        public List<int> ProductIds { get; private set; } = new List<int>();

    }
}