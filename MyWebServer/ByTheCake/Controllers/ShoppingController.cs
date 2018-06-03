using System.Linq;
using MyWebServer.ByTheCake.Data;
using MyWebServer.ByTheCake.Infrastructure;
using MyWebServer.ByTheCake.Models;
using MyWebServer.Server.Http.Contracts;
using MyWebServer.Server.Http.Response;

namespace MyWebServer.ByTheCake.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly CakesData cakesData;

        public ShoppingController()
        {
            this.cakesData = new CakesData();
        }

        public IHttpResponse AddToCart(IHttpRequest req)
        {
            var id = int.Parse(req.UrlParameters["id"]);

            var cake = this.cakesData.Find(id);

            if (cake == null)
            {
                return new NotFoundResponse();
            }

            var shopingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shopingCart.Orders.Add(cake);

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

            if (!shoppingCart.Orders.Any())
            {
                this.ViewData["cartItems"] = "No items in your cart";
                this.ViewData["totalCost"] = "0.00";
            }
            else
            {
                var items = shoppingCart
                    .Orders
                    .Select(i => $"<div>{i.Name} - ${i.Price:f2}</div><br/>");
                var totalCost = shoppingCart
                    .Orders.Sum(i => i.Price);

                this.ViewData["cartItems"] = string.Join(string.Empty, items);
                this.ViewData["totalCost"] = $"{totalCost:f2}";
            }

            return this.FileViewResponse("shopping/cart");
        }

        public IHttpResponse FinishOrder(IHttpRequest req)
        {
            req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey).Orders.Clear();

            return this.FileViewResponse("shopping/finish-order");
        }
    }
}