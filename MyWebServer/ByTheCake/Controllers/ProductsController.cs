using MyWebServer.ByTheCake.Data;
using MyWebServer.ByTheCake.Services;
using MyWebServer.ByTheCake.Services.Contracts;
using MyWebServer.ByTheCake.ViewModels.Products;
using MyWebServer.Server.Http.Contracts;
using MyWebServer.Server.Http.Response;

namespace MyWebServer.ByTheCake.Controllers
{
    using Infrastructure;
    using ViewModels;
    using Server.Http.Contracts;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ProductsController : Controller
    {
        private readonly IProductsService products;

        public ProductsController()
        {
            this.products = new ProductService();
        }
        public IHttpResponse Add()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(@"products\add");
        }

        public IHttpResponse Add(AddProductViewModel model)
        {
            if (model.Name.Length < 3 
                || model.Name.Length > 30 
                || model.Price == 0
                || model.ImageUrl.Length < 3
                || model.ImageUrl.Length > 2000)
            {
                this.AddError("Product information is not valid!");

                this.ViewData["showResult"] = "none";
                return this.FileViewResponse("products/add");

            }

            this.products.Create(model.Name, model.Price, model.ImageUrl);
            
            this.ViewData["name"] = model.Name;
            this.ViewData["price"] = model.Price.ToString();
            this.ViewData["imageUrl"] = model.ImageUrl;
            this.ViewData["showResult"] = "block";

            return this.FileViewResponse(@"products\add");
        }

        public IHttpResponse Search(IHttpRequest req)
        {
            const string searchTermKey = "searchTerm";

            var urlParameters = req.UrlParameters;
            
            this.ViewData["results"] = string.Empty;

            var searchTerm = urlParameters.ContainsKey(searchTermKey)
                ? urlParameters[searchTermKey]
                : null;

            this.ViewData["searchTerm"] = searchTerm;

            var result = this.products.All(searchTerm);

            if (!result.Any())
            {
                this.ViewData["result"] = "No cakes found";
            }
            else
            {
                var allProducts = result.Select(c => $@"<div><a href=""/cakes/{c.Id}"">{c.Name}</a> - ${c.Price:f2}<a href=""/shopping/add/{c.Id}?searchTerm={searchTerm}"">OrderProducts</a></div>");

                var allProductsAsString = string.Join(Environment.NewLine, allProducts);

                this.ViewData["results"] = allProductsAsString;
            }
            
            this.ViewData["showCart"] = "none";

            var shopingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shopingCart.ProductIds.Any())
            {
                var totalProducts = shopingCart.ProductIds.Count;
                var totalProductsText = totalProducts != 1 ? "products" : "product";

                this.ViewData["showCart"] = "block";
                this.ViewData["products"] = $"{totalProducts} {totalProductsText}";
            }


            return this.FileViewResponse(@"products\search");
        }

        public IHttpResponse Detail(int id)
        {
            var product = this.products.Find(id);

            if (product == null)
            {
                return new NotFoundResponse();
            }

            this.ViewData["name"] = product.Name;
            this.ViewData["price"] = product.Price.ToString("f2");
            this.ViewData["imageUrl"] = product.ImageUrl;

            return this.FileViewResponse("products/details");
        }
    }
}
