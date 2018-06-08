using System;
using System.Collections.Generic;
using System.Linq;
using MyWebServer.GameStore.Data;
using MyWebServer.GameStore.Data.Models;
using MyWebServer.GameStore.Services.Contracts;
using Order = MyWebServer.GameStore.Data.Models.Order;

namespace MyWebServer.GameStore.Services
{
    public class ShoppingService : IShoppingService
    {
        public void CreateOrder(int userId, IEnumerable<int> gameIds)
        {
            using (var db = new GameStoreDbContext())
            {
                var order = new Order
                {
                    UserId = userId,
                    Games = gameIds.Select(id => new OrderGame()
                    {
                        GameId = id
                    }).ToList()
                };

                var user = db.Users.First(u => u.Id == userId);

                foreach (var id in gameIds)
                {
                    user.Games.Add(new UserGame{GameId = id});
                }

                order.TotalPrice = db.Games.Where(g => gameIds.Contains(g.Id)).Sum(g => g.Price);
                
                db.Add(order);
                db.SaveChanges();
            }
        }
    }
}