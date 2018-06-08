using System.Collections.Generic;
using System.Linq;
using MyWebServer.GameStore.Data;
using MyWebServer.GameStore.Data.Models;
using MyWebServer.GameStore.Services.Contracts;
using MyWebServer.GameStore.ViewModels;

namespace MyWebServer.GameStore.Services
{
    public class GameService : IGameService
    {
        public bool AddGame(GameViewModel model)
        {
            using (var db = new GameStoreDbContext())
            {
                if (db.Games.Any(g => g.Title == model.Title))
                {
                    return false;
                }
                var game = new Game
                {
                    Title = model.Title,
                    Desctription = model.Desctription,
                    Price = model.Price,
                    ReleaseDate = model.ReleaseDate,
                    Size = model.Size,
                    ThumbnailURL = model.ThumbnailURL,
                    TrailerId = model.TrailerId
                };

                db.Add(game);
                db.SaveChanges();

                return true;
            }
        }

        public bool EditGame(GameViewModel model)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = db.Games.FirstOrDefault(g => g.Title == model.Title);
                if (game == null)
                {
                    return false;
                }
                game.Title = model.Title;
                game.Desctription = model.Desctription;
                game.Price = model.Price;
                game.ReleaseDate = model.ReleaseDate;
                game.Size = model.Size;
                game.ThumbnailURL = model.ThumbnailURL;
                game.TrailerId = model.TrailerId;
                
                db.SaveChanges();

                return true;
            }
        }

        public bool DeleteGame(string title)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = db.Games.FirstOrDefault(g => g.Title == title);
                if (game == null)
                {
                    return false;
                }

                db.Remove(game);
                db.SaveChanges();

                return true;
            }
        }

        public GameViewModel FindGame(string title)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = db.Games.FirstOrDefault(g => g.Title == title);
                if (game == null)
                {
                    return null;
                }
                
                var gameModel = new GameViewModel
                {
                    Title = game.Title,
                    Desctription = game.Desctription,
                    Price = game.Price,
                    ReleaseDate = game.ReleaseDate,
                    Size = game.Size,
                    ThumbnailURL = game.ThumbnailURL,
                    TrailerId = game.TrailerId
                };

                return gameModel;
            }
        }

        public int? GetGameId(string title)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = db.Games.FirstOrDefault(g => g.Title == title);

                return game?.Id;
            }
        }

        public List<GameViewModel> FindProductsInCart(List<int> shoppingCartProductIds)
        {
            var games = new List<GameViewModel>();
            using (var db = new GameStoreDbContext())
            {
                games = db.Games
                    .Where(g => shoppingCartProductIds.Contains(g.Id))
                    .Select(g => new GameViewModel
                    {
                        Title = g.Title,
                        Desctription = g.Desctription,
                        Price = g.Price,
                        ReleaseDate = g.ReleaseDate,
                        Size = g.Size,
                        ThumbnailURL = g.ThumbnailURL,
                        TrailerId = g.TrailerId
                    }).AsEnumerable().ToList();
            }

            return games;
        }

        public IEnumerable<GameViewModel> AllGames()
        {
            var games = new List<GameViewModel>();
            using (var db = new GameStoreDbContext())
            {
                games = db.Games.Select(game => new GameViewModel
                {
                    Title = game.Title,
                    Desctription = game.Desctription,
                    Price = game.Price,
                    ReleaseDate = game.ReleaseDate,
                    Size = game.Size,
                    ThumbnailURL = game.ThumbnailURL,
                    TrailerId = game.TrailerId
                }).AsEnumerable().ToList();

            }

            return games;
        }
        
    }
}