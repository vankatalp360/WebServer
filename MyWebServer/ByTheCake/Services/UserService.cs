using System;
using System.Collections.Generic;
using System.Linq;
using MyWebServer.ByTheCake.Data;
using MyWebServer.ByTheCake.Data.Models;
using MyWebServer.ByTheCake.Services.Contracts;
using MyWebServer.ByTheCake.ViewModels.Account;

namespace MyWebServer.ByTheCake.Services
{
    public class UserService : IUserService
    {
        public bool Create(string username, string password)
        {
            using (var db = new ByTheCakeDbContext())
            {
                if (db.Users.Any(u => u.Username == username))
                {
                    return false;
                }

                var user = new User
                {
                    Username = username,
                    Password = password,
                    RegistrationDate = DateTime.UtcNow
                };

                db.Add(user);
                db.SaveChanges();
                return true;
            }
        }

        public bool FindUser(string username, string password)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db
                    .Users
                    .Any(u => u.Username == username && u.Password == password);

            }
        }

        public ProfileVewModel Profile(string username)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db
                    .Users
                    .Where(u => u.Username == username)
                    .Select(u => new ProfileVewModel
                    {
                        Username = u.Username,
                        RegitrationDate = u.RegistrationDate,
                        TotalOrders = u.Orders.Count()
                    })
                    .FirstOrDefault();
            }
        }

        public IEnumerable<OrdersListingViewModel> Orders(string username)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var orders = db
                    .Orders
                    .Where(o => o.User.Username == username);

                return orders
                    .Select(o => new OrdersListingViewModel
                    {
                        Id = o.Id,
                        CreatedData = o.CreationDate,
                        Sum = o.Products.Sum(p => p.Product.Price)
                    }).ToList();
            }
        }

        public int? GetUserId(string username)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var id = db
                    .Users
                    .Where(u => u.Username == username)
                    .Select(u => u.Id)
                    .FirstOrDefault();

                return id != 0 ? (int?)id : null;
            }
        }

        public IEnumerable<OrderProductsViewModel> OrderProducts(int id)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var orderProducts = db.OrderProducts.Where(op => op.OrderId == id);

                var products =
                    orderProducts.Select(op => op.Product)
                        .Select(p => new OrderProductsViewModel
                        {
                            Name = p.Name,
                            Price = p.Price
                        });
                

                return products
                    .Select(p => new OrderProductsViewModel
                    {
                        Name = p.Name,
                        Price = p.Price
                    }).ToList();
            }
        }
        
    }
}