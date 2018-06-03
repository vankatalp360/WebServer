using MyWebServer.ByTheCake.Data;
using MyWebServer.Server.Http.Contracts;

namespace MyWebServer.ByTheCake.Controllers
{
    using Infrastructure;
    using Models;
    using Server.Http.Contracts;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class CakesController : Controller
    {
        private readonly CakesData cakesData;

        public CakesController()
        {
            this.cakesData = new CakesData();
        }
        public IHttpResponse Add()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(@"cakes\add");
        }

        public IHttpResponse Add(string name, string price)
        {
            var cake = new Cake
            {
                Name = name,
                Price = decimal.Parse(price)
            };
            
            this.cakesData.Add(name, price);

            this.ViewData["name"] = name;
            this.ViewData["price"] = price;
            this.ViewData["showResult"] = "block";

            return this.FileViewResponse(@"cakes\add");
        }

        public IHttpResponse Search(IHttpRequest req)
        {
            const string searchTermKey = "searchTerm";

            var urlParameters = req.UrlParameters;
            
            this.ViewData["results"] = string.Empty;
            this.ViewData["searchTerm"] = string.Empty  ;

            if (urlParameters.ContainsKey(searchTermKey))
            {
                var searchTerm = urlParameters[searchTermKey];

                this.ViewData["searchTerm"] = searchTerm;

                var savedCakesDivs = this.cakesData.All()
                    .Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()))
                    .Select(c => $@"<div>{c.Name} - ${c.Price:f2}<a href=""/shopping/add/{c.Id}?searchTerm={searchTerm}"">Order</a></div>");

                var results = string.Join(Environment.NewLine, savedCakesDivs);

                this.ViewData["results"] = results;
            }
            this.ViewData["showCart"] = "none";

            var shopingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shopingCart.Orders.Any())
            {
                var totalProducts = shopingCart.Orders.Count;
                var totalProductsText = totalProducts != 1 ? "products" : "product";

                this.ViewData["showCart"] = "block";
                this.ViewData["products"] = $"{totalProducts} {totalProductsText}";
            }


            return this.FileViewResponse(@"cakes\search");
        }
    }
}
