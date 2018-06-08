using System;
using Microsoft.EntityFrameworkCore;
using MyWebServer.GameStore.Controllers;
using MyWebServer.GameStore.Data;
using MyWebServer.GameStore.ViewModels;
using MyWebServer.Server.Contracts;
using MyWebServer.Server.Routing.Contracts;

namespace MyWebServer.GameStore
{
    public class GameStoreApp : IApplication
    {
        public void InitializeDatabase()
        {
            using (var db = new GameStoreDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.AnonymousPaths.Add("/account/login");
            appRouteConfig.AnonymousPaths.Add("/account/register");
            appRouteConfig.AnonymousPaths.Add("/home");
            appRouteConfig.AnonymousPaths.Add("/");
            appRouteConfig.AnonymousPaths.Add("game/details/{(?<title>[A-Za-z0-9% ]+)}");

            appRouteConfig
                .Get(
                    "/home",
                    req => new GameController().AllGames(req));
            
            appRouteConfig
                .Get(
                    "/",
                    req => new GameController().AllGames(req));
            

            appRouteConfig
                .Get(
                    "account/register",
                    req => new AccountController().Register(req));

            appRouteConfig
                .Post(
                    "account/register",
                    req => new AccountController().Register(new RegisterViewModel
                    {
                        Name = req.FormData["name"],
                        Email = req.FormData["email"],
                        Password = req.FormData["password"],
                        ConfirmPassword = req.FormData["confirmPassword"],
                    }));

            appRouteConfig
                .Get(
                    "account/login",
                    req => new AccountController().Login(req));

            appRouteConfig
                .Post(
                    "account/login",
                    req => new AccountController().Login(req, new LoginViewModel
                    {
                        Email = req.FormData["email"],
                        Password = req.FormData["password"]
                    }));

            appRouteConfig
                .Get(
                    "game/add",
                    req => new GameController().Add(req));

            appRouteConfig
                .Post(
                    "game/add",
                    req => new GameController().Add(req, new GameViewModel
                    {
                        Title = req.FormData["title"],
                        Desctription = req.FormData["description"],
                        Price = decimal.Parse(req.FormData["price"]),
                        Size = decimal.Parse(req.FormData["size"]),
                        ReleaseDate = DateTime.Parse(req.FormData["releaseDate"]),
                        TrailerId = req.FormData["youtubeUrl"],
                        ThumbnailURL = req.FormData["thumbnail"],
                    }));

            appRouteConfig
                .Get(
                    "game/edit/{(?<title>[A-Za-z0-9% ]+)}",
                    req => new GameController().Edit(req, req.UrlParameters["title"]));

            appRouteConfig
                .Post(
                    "game/edit/{(?<title>[A-Za-z0-9% ]+)}",
                    req => new GameController().Edit(req, new GameViewModel
                    {
                        Title = req.FormData["title"],
                        Desctription = req.FormData["description"],
                        Price = decimal.Parse(req.FormData["price"]),
                        Size = decimal.Parse(req.FormData["size"]),
                        ReleaseDate = DateTime.Parse(req.FormData["releaseDate"]),
                        TrailerId = req.FormData["youtubeUrl"],
                        ThumbnailURL = req.FormData["thumbnail"],
                    }));

            appRouteConfig
                .Get(
                    "game/details/{(?<title>[A-Za-z0-9% ]+)}",
                    req => new GameController().Details(req, req.UrlParameters["title"]));

            appRouteConfig
                .Get(
                    "game/delete/{(?<title>[A-Za-z0-9% ]+)}",
                    req => new GameController().Delete(req.UrlParameters["title"], req));

            appRouteConfig
                .Post(
                    "game/delete/{(?<title>[A-Za-z0-9% ]+)}",
                    req => new GameController().Delete(req, req.UrlParameters["title"]));

            appRouteConfig
                .Get(
                    "games/all",
                    req => new GameController().AdminGames(req));

            appRouteConfig
                .Get(
                    "logout",
                    req => new AccountController().Logout(req));

            appRouteConfig
                .Get(
                    "shopping/cart",
                    req => new ShoppingController().ShowCart(req));

            appRouteConfig
                .Post(
                    "shopping/cart",
                    req => new ShoppingController().FinishOrder(req));
            
            appRouteConfig
                .Get(
                    "shopping/add/{(?<title>[A-Za-z0-9% ]+)}",
                    req => new ShoppingController().AddToCart(req));

            appRouteConfig
                .Get(
                    "shopping/remove/{(?<title>[A-Za-z0-9% ]+)}",
                    req => new ShoppingController().RemoveFromCart(req));

        }

    }
}