using System;
using System.Collections.Generic;
using System.Linq;
using MyWebServer.ByTheCake.Data;
using MyWebServer.ByTheCake.Data.Models;
using MyWebServer.ByTheCake.Services.Contracts;
using MyWebServer.ByTheCake.ViewModels.Products;

namespace MyWebServer.ByTheCake.Services
{
    public class ProductService : IProductsService
    {
        public void Create(string name, decimal price, string imageUrl)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var product = new Product
                {
                    Name = name,
                    Price = price,
                    ImageUrl = imageUrl
                };

                db.Add(product);
                db.SaveChanges();

            }
        }

        public IEnumerable<ProductListingViewModel> All(string searchTerm = null)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var resultsQuery = db.Products.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    resultsQuery = resultsQuery
                        .Where(pr => pr.Name.ToLower().Contains(searchTerm.ToLower()));
                }

                return resultsQuery
                    .Select(pr => new ProductListingViewModel
                    {
                        Id = pr.Id,
                        Name = pr.Name,
                        Price = pr.Price,
                    })
                    .ToList();
            }
        }

        public ProductDetailsVewModel Find(int id)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products
                    .Where(pr => pr.Id == id)
                    .Select(pr => new ProductDetailsVewModel
                    {
                        Name = pr.Name,
                        Price = pr.Price,
                        ImageUrl = pr.ImageUrl
                    })
                    .FirstOrDefault();
            }
        }

        public bool Exists(int id)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products.Any(p => p.Id == id);
            }
        }

        public IEnumerable<ProductInCartVewModel> FindProductsInCart(IEnumerable<int> ids)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products
                    .Where(p => ids.Contains(p.Id))
                    .Select(p => new ProductInCartVewModel
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                    .ToList();
            }
        }
    }
}