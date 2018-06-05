using System;
using System.Collections.Generic;
using System.Linq;
using MyWebServer.ByTheCake.Data;
using MyWebServer.ByTheCake.Data.Models;
using MyWebServer.ByTheCake.Services.Contracts;

namespace MyWebServer.ByTheCake.Services
{
    public class ShoppingService : IShoppingService
    {
        public void CreateOrder(int userId, IEnumerable<int> productIds)
        {
            using (var db = new ByTheCakeDbContext())       
            {
                var order = new Order
                {
                    UserId = userId,
                    CreationDate = DateTime.UtcNow,
                    Products = productIds.Select(id => new OrderProduct
                    {
                        ProductId = id
                    }).ToList()
                };

                db.Add(order);
                db.SaveChanges();
            }
        }
    }
}