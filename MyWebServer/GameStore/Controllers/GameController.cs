using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using MyWebServer.GameStore.Services;
using MyWebServer.GameStore.Services.Contracts;
using MyWebServer.GameStore.ViewModels;
using MyWebServer.Infrastructure;
using MyWebServer.Server.Http;
using MyWebServer.Server.Http.Contracts;
using MyWebServer.Server.Http.Response;

namespace MyWebServer.GameStore.Controllers
{
    public class GameController : GameStoreController
    {
        private const string AddView = "/game/add";
        private const string EditView = "/game/edit";
        private IGameService games;

        public GameController()
        {
            this.games = new GameService();
        }

        public IHttpResponse Add(IHttpRequest request)
        {
            var isAdmin = this.SetIdentity(request);

            if (!isAdmin)
            {

                return new RedirectResponse("/home");
            }

            return this.FileViewResponse("/game/add");
        }

        public IHttpResponse Add(IHttpRequest request, GameViewModel model)
        {
            var isAdmin = this.SetIdentity(request);

            if (!isAdmin)
            {
                return new RedirectResponse("/home");
            }

            var error = ValidateModel(model);
            if (error != null)
            {
                AddError(error);
                return this.FileViewResponse(AddView);
            }

            var success = this.games.AddGame(model);

            if (!success)
            {
                error = $"This game is already added!";
                this.AddError(error);
                return this.FileViewResponse(AddView);
            }

            return new RedirectResponse("/");
        }

        public IHttpResponse Edit(IHttpRequest request, string title)
        {
            var isAdmin = this.SetIdentity(request);

            if (!isAdmin)
            {
                return new RedirectResponse("/home");
            }

            var game = this.games.FindGame(WebUtility.UrlDecode(title));

            if (game == null)
            {
                var error = $"Invalid Game!";
                this.AddError(error);
                return this.FileViewResponse(EditView);
            }

            this.ViewData["title"] = game.Title;
            this.ViewData["description"] = game.Desctription;
            this.ViewData["price"] = game.Price.ToString();
            this.ViewData["size"] = game.Size.ToString();
            this.ViewData["releaseDate"] = game.ReleaseDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            this.ViewData["youtubeUrl"] = game.TrailerId;
            this.ViewData["thumbnail"] = game.ThumbnailURL;

            return this.FileViewResponse("/game/edit");
        }

        public IHttpResponse Edit(IHttpRequest request, GameViewModel model)
        {
            var isAdmin = this.SetIdentity(request);

            if (!isAdmin)
            {
                return new RedirectResponse("/home");
            }

            var error = ValidateModel(model);
            if (error != null)
            {
                AddError(error);
                return this.FileViewResponse(EditView);
            }

            var success = this.games.EditGame(model);

            if (!success)
            {
                error = $"Invalid Game!";
                this.AddError(error);
                return this.FileViewResponse(EditView);
            }

            return new RedirectResponse("/");
        }
        public IHttpResponse Delete(string title, IHttpRequest request)
        {
            var isAdmin = this.SetIdentity(request);

            if (!isAdmin)
            {
                return new RedirectResponse("/home");
            }

            var game = this.games.FindGame(WebUtility.UrlDecode(title));

            if (game == null)
            {
                var error = $"Invalid Game!";
                this.AddError(error);
                return this.FileViewResponse("/home");
            }

            this.ViewData["title"] = game.Title;
            this.ViewData["description"] = game.Desctription;
            this.ViewData["price"] = game.Price.ToString();
            this.ViewData["size"] = game.Size.ToString();
            this.ViewData["releaseDate"] = game.ReleaseDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            this.ViewData["youtubeUrl"] = game.TrailerId;
            this.ViewData["thumbnail"] = game.ThumbnailURL;

            return this.FileViewResponse("/game/delete");
        }
        
        public IHttpResponse Delete(IHttpRequest request, string title)
        {
            var isAdmin = this.SetIdentity(request);

            if (!isAdmin)
            {
                return new RedirectResponse("/home");
            }

            var success = this.games.DeleteGame(WebUtility.UrlDecode(title));

            if (!success)
            {
                var error = $"Invalid Game!";
                this.AddError(error);
                return this.FileViewResponse("/home");
            }

            return new RedirectResponse("/");
        }

        public IHttpResponse AdminGames(IHttpRequest request)
        {
            var isAdmin = this.SetIdentity(request);

            if (!isAdmin)
            {
                return new RedirectResponse("/home");
            }

            var allGames = games.AllGames();
            
            var gamesResult = new StringBuilder();

            var counter = 1;
            foreach (var game in allGames)
            {
                gamesResult.AppendLine($@"<tr> <th scope=""row"">{counter}</th> <td>{game.Title}</td> <td>{game.Size:f1} GB</td> <td>{game.Price:f2} &euro;</td> <td> <a href = ""/game/edit/{game.Title}"" class=""btn btn-warning btn-sm"">Edit</a> <a href = ""/game/delete/{game.Title}"" class=""btn btn-danger btn-sm"">Delete</a> </tr>");
                counter++;
            }


            this.ViewData["games"] = gamesResult.ToString();
            return this.FileViewResponse("/admin/games");
        }

        public IHttpResponse AllGames(IHttpRequest request)
        {
            var isAdmin = this.SetIdentity(request);
            
            var allGames = games.AllGames();

            var gamesResult = new StringBuilder();

            if (isAdmin == true)
            {
                foreach (var game in allGames)
                {
                    gamesResult.AppendLine($@"
            <div class=""card col - 4 thumbnail"">
                <img class=""card-image-top img-fluid img-thumbnail"" onerror =""this.src = '{game.ThumbnailURL}';"" src =""{game.ThumbnailURL}"">

                <div class=""card-body"">
                    <h4 class=""card-title"">{game.Title}</h4>
                    <p class=""card-text""><strong>Price</strong> - {game.Price:f2}&euro;
                    </p>
                    <p class=""card-text""><strong>Size</strong> - {game.Size:f1} GB
                    </p>
                    <p class=""card-text"" style=""text-overflow: ellipsis"">{game.Desctription}</p>
                </div>

                <div class=""card-footer"">
                    <a class=""card-button btn btn-warning"" name =""edit"" href =""/game/edit/{game.Title}"">Edit</a>
                    <a class=""card-button btn btn-danger"" name =""delete"" href =""/game/dalete/{game.Title}"">Delete</a>

                    <a class=""card-button btn btn-outline-primary"" name=""info"" href =""/game/details/{game.Title}"">Info</a>
                    <a class=""card-button btn btn-primary"" name =""buy"" href =""/shopping/add/{game.Title}"">Buy</a>
                </div>

            </div>");
                }
            }
            else
            {
                foreach (var game in allGames)
                {
                    gamesResult.AppendLine($@"
            <div class=""card col - 4 thumbnail"">
                <img class=""card-image-top img-fluid img-thumbnail"" onerror =""this.src = '{game.ThumbnailURL}';"" src =""{game.ThumbnailURL}"">

                <div class=""card-body"">
                    <h4 class=""card-title"">{game.Title}</h4>
                    <p class=""card-text""><strong>Price</strong> - {game.Price:f2}&euro;
                    </p>
                    <p class=""card-text""><strong>Size</strong> - {game.Size:f1} GB
                    </p>
                    <p class=""card-text"" style=""text-overflow: ellipsis"">{game.Desctription}</p>
                </div>

                <div class=""card-footer"">

                    <a class=""card-button btn btn-outline-primary"" name=""info"" href =""/game/details/{game.Title}"">Info</a>
                    <a class=""card-button btn btn-primary"" name =""buy"" href =""/shopping/add/{game.Title}"">Buy</a>
                </div>

            </div>");
                }
            }
            
            this.ViewData["games"] = gamesResult.ToString();
            return this.FileViewResponse("/home");
        }
        

        public IHttpResponse Details(IHttpRequest request, string title)
        {
            var isAdmin = this.SetIdentity(request);
            

            var game = this.games.FindGame(WebUtility.UrlDecode(title));

            if (game == null)
            {
                var error = $"Invalid Game!";
                this.AddError(error);
                return this.FileViewResponse("/home");
            }

            this.ViewData["title"] = game.Title;
            this.ViewData["description"] = game.Desctription;
            this.ViewData["price"] = game.Price.ToString();
            this.ViewData["size"] = game.Size.ToString();
            this.ViewData["youtubeUrl"] = game.TrailerId;
            this.ViewData["releaseDate"] = game.ReleaseDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (isAdmin)
            {
                this.ViewData["buttons"] = $@"
                <a class=""btn btn-outline - primary"" href= ""/home"">Back</a>
                <a class=""btn btn-warning"" href=""/game/edit"">Edit</a>
                <a class=""btn btn-danger"" href =""/game/delete"">Delete</a>
                <a class=""btn btn-primary"" href=""/shopping/add/{game.Title}"">Buy</a>";
            }
            else
            {
                this.ViewData["buttons"] = $@"
                <a class=""btn btn-outline - primary"" href= ""/home"">Back</a>
                <a class=""btn btn-primary"" href=""/shopping/add/{game.Title}"">Buy</a>";
            }

            return this.FileViewResponse("/game/details");
        }
    }
}