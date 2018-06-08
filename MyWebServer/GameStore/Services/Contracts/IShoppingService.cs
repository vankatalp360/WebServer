using System.Collections.Generic;

namespace MyWebServer.GameStore.Services.Contracts
{
    public interface IShoppingService
    {
        void CreateOrder(int userId, IEnumerable<int> gameIds);
    }
}