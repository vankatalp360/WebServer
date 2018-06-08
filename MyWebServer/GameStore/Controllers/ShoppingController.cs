using System;
using System.Linq;
using System.Net;
using MyWebServer.ByTheCake.ViewModels;
using MyWebServer.GameStore.Services;
using MyWebServer.GameStore.Services.Contracts;
using MyWebServer.Server.Http;
using MyWebServer.Server.Http.Contracts;
using MyWebServer.Server.Http.Response;

namespace MyWebServer.GameStore.Controllers
{
    public class ShoppingController : GameStoreController
    {
        private readonly IGameService games;
        private readonly IUserService users;
        private readonly IShoppingService shopping;

        public ShoppingController()
        {
            this.games = new GameService();
            this.users = new UserService();
            this.shopping = new ShoppingService();
        }

        public IHttpResponse AddToCart(IHttpRequest req)
        {
            var isAdmin = this.SetIdentity(req);
            var title = req.UrlParameters["title"];

            var game = this.games.FindGame(WebUtility.UrlDecode(title));
            var gameId = this.games.GetGameId(WebUtility.UrlDecode(title));

            if (game == null || gameId == null)
            {
                this.AddError("Invalid Game!");

                return this.FileViewResponse("/home");
            }

            var id = gameId.Value;
            var shopingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shopingCart.ProductIds.Add(id);

            var redirectUrl = "/home";
            
            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse RemoveFromCart(IHttpRequest req)
        {
            var isAdmin = this.SetIdentity(req);
            var title = req.UrlParameters["title"];

            var game = this.games.FindGame(WebUtility.UrlDecode(title));
            var gameId = this.games.GetGameId(WebUtility.UrlDecode(title));

            if (game == null || gameId == null)
            {
                this.AddError("Invalid Game!");

                return this.FileViewResponse("/");
            }

            var id = gameId.Value;
            var shopingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shopingCart.ProductIds.Remove(id);

            var redirectUrl = "/shopping/cart";

            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse ShowCart(IHttpRequest req)
        {
            var isAdmin = this.SetIdentity(req);
            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.ProductIds.Any())
            {
                this.ViewData["games"] = "No items in your cart";
                this.ViewData["totalCost"] = "0.00";
            }
            else
            {
                var productsInCart = this.games.FindProductsInCart(shoppingCart.ProductIds);


                var items = productsInCart
                    .Select(pr => $@"<div class=""list - group - item"">
                    <div class=""media"">
                        <a class=""btn btn-outline-danger btn-lg align-self-center mr-3"" href =""/shopping/remove/{WebUtility.UrlDecode(pr.Title)}"" >X</a>
                        <img class=""d-flex mr-4 align-self-center img-thumbnail"" height =""127"" src =""{pr.ThumbnailURL}""
                             width= ""227"" alt=""Generic placeholder image"">
                        <div class=""media-body align-self-center"" >
                            <a href = ""#"">
                                <h4 class=""mb-1 list-group-item-heading"" >{pr.Title}</h4>
                            </a>
                            <p>{pr.Desctription}</p>
                        </div>
                        <div class=""col-md-2 text-center align-self-center mr-auto"">
                            <h2> {pr.Price}&euro; </h2>
                        </div>
                    </div>
                </div>");

                var totalCost = productsInCart
                    .Sum(pr => pr.Price);

                this.ViewData["games"] = string.Join(string.Empty, items);
                this.ViewData["totalCost"] = $"{totalCost:f2}";
            }

            return this.FileViewResponse("account/cart");
        }

        public IHttpResponse FinishOrder(IHttpRequest req)
        {
            var isAdmin = this.SetIdentity(req);
            var email = req.Session.Get<string>(SessionStore.CurrentUserKey);
            var shopingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            var userId = this.users.GatUserId(email);
            if (userId == null)
            {
                throw new InvalidOperationException($"User {email} does not exist!");
            }

            var productIds = shopingCart.ProductIds;
            if (!productIds.Any())
            {
                return new RedirectResponse("/");
            }

            this.shopping.CreateOrder(userId.Value, productIds);

            shopingCart.ProductIds.Clear();

            this.ViewData["games"] = "Thank you for your purchase!";
            this.ViewData["totalCost"] = "0.00";

            return this.FileViewResponse("account/cart");
        }
    }
}