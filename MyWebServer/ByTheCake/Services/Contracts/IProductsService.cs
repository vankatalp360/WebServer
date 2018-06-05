using System.Collections.Generic;
using MyWebServer.ByTheCake.ViewModels.Products;

namespace MyWebServer.ByTheCake.Services.Contracts
{
    public interface IProductsService
    {
        void Create(string name, decimal price, string imageUrl);

        IEnumerable<ProductListingViewModel> All(string searchTerm = null);

        ProductDetailsVewModel Find(int id);

        bool Exists(int id);

        IEnumerable<ProductInCartVewModel> FindProductsInCart(IEnumerable<int> ids);
    }
}