using Microsoft.EntityFrameworkCore;
using MyWebServer.ByTheCake.Data;
using MyWebServer.ByTheCake.ViewModels.Account;
using MyWebServer.ByTheCake.ViewModels.Products;
using MyWebServer.Server.Contracts;
using MyWebServer.Server.Routing.Contracts;

namespace MyWebServer.ByTheCake
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class ByTheCakeApp : IApplication
    {
        public void InitializeDatabase()
        {
            using (var db = new ByTheCakeDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .Get("/", req => new HomeController().Index());

            appRouteConfig
                .Get("/about", req => new HomeController().About());

            appRouteConfig
                .Get("/add", req => new ProductsController().Add());

            appRouteConfig
                .Get("/login", req => new AccountController().Login());

            appRouteConfig.Post(
                "/logout",
                req => new AccountController().Logout(req));

            appRouteConfig
                .Post("/login", 
                    req => new AccountController().Login(req, new LoginViewModel
                    {
                        Username = req.FormData["name"],
                        Password = req.FormData["password"]
                    }));

            appRouteConfig
                .Get("/register",
                    req => new AccountController().Register());

            appRouteConfig
                .Post("/register",
                    req => new AccountController().Register(req, new RegisterUserViewModel
                    {
                        Username = req.FormData["username"],
                        Password = req.FormData["password"],
                        ConfirmParssowrd = req.FormData["confirm-Password"]
                    }));

            appRouteConfig
                .Get("/profile",
                req => new AccountController().Profile(req));

            appRouteConfig
                .Get("/orders",
                req => new AccountController().Orders(req));

            appRouteConfig
                .Post(
                    "/add",
                    req => new ProductsController().Add(new AddProductViewModel
                    {
                        Name = req.FormData["name"],
                        Price = decimal.Parse(req.FormData["price"]),
                        ImageUrl = req.FormData["imageUrl"]
                    }));

            appRouteConfig
                .Get("/cakes/{(?<id>[0-9]+)}",
                req => new ProductsController()
                .Detail(int.Parse(req.UrlParameters["id"])));

            appRouteConfig
                .Get("/orderDetails/{(?<id>[0-9]+)}",
                    req => new AccountController()
                        .Order(req, int.Parse(req.UrlParameters["id"])));

            appRouteConfig
                .Get(
                    "/search", 
                    req => new ProductsController().Search(req));

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
