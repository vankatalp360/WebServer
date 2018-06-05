using System.Collections.Generic;
using MyWebServer.ByTheCake.ViewModels.Account;

namespace MyWebServer.ByTheCake.Services.Contracts
{
    public interface IUserService
    {
        bool Create(string username, string password);

        bool FindUser(string username, string password);

        ProfileVewModel Profile(string username);

        int? GetUserId(string username);

        IEnumerable<OrdersListingViewModel> Orders(string username);

        IEnumerable<OrderProductsViewModel> OrderProducts(int id);
    }
}