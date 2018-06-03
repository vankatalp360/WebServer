using MyWebServer.Server.Contracts;
using MyWebServer.Server.Routing.Contracts;

namespace MyWebServer.ByTheCake
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class ByTheCakeApp : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .Get("/", req => new HomeController().Index());

            appRouteConfig
                .Get("/about", req => new HomeController().About());

            appRouteConfig
                .Get("/add", req => new CakesController().Add());

            appRouteConfig
                .Get("login", req => new AccountController().Login());

            appRouteConfig.Post(
                "/logout",
                req => new AccountController().Logout(req));

            appRouteConfig
                .Post("login", 
                    req => new AccountController().Login(req));

            appRouteConfig
                .Post(
                    "/add",
                    req => new CakesController().Add(req.FormData["name"], req.FormData["price"]));

            appRouteConfig
                .Get(
                    "/search", 
                    req => new CakesController().Search(req));

            appRouteConfig
                .Get(
                "/shopping/add/{(?<id>[0-9]+)}"
                ,req => new ShoppingController()
                .AddToCart(req));

            appRouteConfig.Get(
                "cart",
                req => new ShoppingController().ShowCart(req));

            appRouteConfig.Post(
                "/shopping/finish-order",
                req => new ShoppingController().FinishOrder(req));

        }
    }
}
