using System.Collections.Generic;

namespace MyWebServer.ByTheCake.Services.Contracts
{
    public interface IShoppingService
    {
        void CreateOrder(int userId, IEnumerable<int> productIds);
    }
}