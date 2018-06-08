using System.Linq;
using System.Runtime.Serialization.Formatters;
using MyWebServer.GameStore.Data;
using MyWebServer.GameStore.Data.Models;
using MyWebServer.GameStore.Services.Contracts;

namespace MyWebServer.GameStore.Services
{
    public class UserService : IUserService

    {
        public bool Create(string email, string name, string password)
        {
            using (var db = new GameStoreDbContext())
            {
                if (db.Users.Any(u => u.Email == email))
                {
                    return false;
                }

                var isAdmin = !db.Users.Any();

                var user = new User
                {
                    Email = email,
                    Name = name,
                    Password = password,
                    IsAdministrator = isAdmin
                };

                db.Add(user);
                db.SaveChanges();
            }

            return true;
        }

        public bool FindUser(string email, string password)
        {
            using (var db = new GameStoreDbContext())
            {
                if (!db.Users.Any(u => u.Email == email && u.Password == password))
                {
                    return false;
                }

                return true;
            }
        }

        public bool IsAdmin(string email)
        {
            using (var db = new GameStoreDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    return false;
                }
                if (!user.IsAdministrator)
                {
                    return false;
                }
                return true;
            }
        }

        public int? GatUserId(string email)
        {
            int? id;
            using (var db = new GameStoreDbContext())
            {
                id = db.Users.FirstOrDefault(u => u.Email == email)?.Id;
            }


            return id;
        }
    }
}