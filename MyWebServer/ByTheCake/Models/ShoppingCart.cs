using System.Collections.Generic;

namespace MyWebServer.ByTheCake.Models
{
    public class ShoppingCart
    {
        public const string SessionKey = "%^Current_Shopping_Cart^%";

        public List<Cake> Orders { get; private set; } = new List<Cake>();

    }
}