using System;
using System.Linq;
using MyWebServer.ByTheCake.Data;
using MyWebServer.Infrastructure;
using MyWebServer.ByTheCake.Services;
using MyWebServer.ByTheCake.Services.Contracts;
using MyWebServer.ByTheCake.ViewModels;
using MyWebServer.Server.Http;
using MyWebServer.Server.Http.Contracts;
using MyWebServer.Server.Http.Response;

namespace MyWebServer.ByTheCake.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly IProductsService products;
        private readonly IUserService users;
        private readonly IShoppingService shopping;

        public ShoppingController()
        {
            this.users = new UserService();
            this.products = new ProductService();
            this.shopping = new ShoppingService();
        }
        public IHttpResponse AddToCart(IHttpRequest req)
        {
            var id = int.Parse(req.UrlParameters["id"]);

            var productExists = this.products.Exists(id);

            if (!productExists)
            {
                return new NotFoundResponse();
            }

            var shopingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shopingCart.ProductIds.Add(id);

            var redirectUrl = "/search";

            const string searchTermKey = "searchTerm";

            if (req.UrlParameters.ContainsKey(searchTermKey))
            {
                redirectUrl = $"{redirectUrl}?{searchTermKey}={req.UrlParameters[searchTermKey]}";
            }

            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse ShowCart(IHttpRequest req)
        {
            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.ProductIds.Any())
            {
                this.ViewData["cartItems"] = "No items in your cart";
                this.ViewData["totalCost"] = "0.00";
            }
            else
            {
                var productsInCart = this.products.FindProductsInCart(shoppingCart.ProductIds);


                var items = productsInCart
                    .Select(pr => $"<div>{pr.Name} - ${pr.Price:f2}</div><br/>");

                var totalCost = productsInCart
                    .Sum(pr => pr.Price);

                this.ViewData["cartItems"] = string.Join(string.Empty, items);
                this.ViewData["totalCost"] = $"{totalCost:f2}";
            }

            return this.FileViewResponse("shopping/cart");
        }

        public IHttpResponse FinishOrder(IHttpRequest req)
        {
            var username = req.Session.Get<string>(SessionStore.CurrentUserKey);
            var shopingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            var userId = this.users.GetUserId(username);
            if (userId == null)
            {
                throw new InvalidOperationException($"User {username} does not exist!");
            }

            var productIds = shopingCart.ProductIds;
            if (!productIds.Any())
            {
                return new RedirectResponse("/");
            }

            this.shopping.CreateOrder(userId.Value, productIds);

            shopingCart.ProductIds.Clear();

            return this.FileViewResponse("shopping/finish-order");
        }
    }
}