using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MyWebServer.ByTheCake.Infrastructure;
using MyWebServer.ByTheCake.Services;
using MyWebServer.ByTheCake.Services.Contracts;
using MyWebServer.ByTheCake.ViewModels;
using MyWebServer.ByTheCake.ViewModels.Account;
using MyWebServer.Server.Http;
using MyWebServer.Server.Http.Contracts;
using MyWebServer.Server.Http.Response;

namespace MyWebServer.ByTheCake.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService users;

        public AccountController()
        {
            this.users = new UserService();
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();
            return this.FileViewResponse(@"\register");
        }

        public IHttpResponse Register(IHttpRequest request, RegisterUserViewModel model)
        {
            this.SetDefaultViewData();

            if (model.Username.Length < 3
                || model.Password.Length < 3
                || model.ConfirmParssowrd != model.Password)
            {
                this.AddError("Invalid user details");

                return this.FileViewResponse("/register");
            }

            var success = this.users.Create(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.AddError("This username is taken!");

                return this.FileViewResponse("/register");
            }
        }


        public IHttpResponse Login()
        {
            this.SetDefaultViewData();
            return this.FileViewResponse("/login");
        }

        public IHttpResponse Login(IHttpRequest request, LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username)
                || string.IsNullOrWhiteSpace(model.Password))
            {
                this.AddError("You have empty fields");

                return this.FileViewResponse("/login");
            }

            var success = this.users.FindUser(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);
                var response = new RedirectResponse("/");
                return response;
            }
            else
            {
                this.AddError("Invalid user details!");
                this.ViewData["authDisplay"] = "none";

                return this.FileViewResponse("/login");
            }
        }

        public IHttpResponse Profile(IHttpRequest request)
        {
            if (!request.Session.Contains(SessionStore.CurrentUserKey))
            {
                throw new InvalidOperationException("There is no logged in user.");
            }

            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);

            var profile = this.users.Profile(username);

            if (profile == null)
            {
                throw new InvalidOperationException($"The user {username} could not be found in the database!");
            }

            this.ViewData["username"] = profile.Username;
            this.ViewData["registrationDate"] = profile.RegitrationDate.ToShortDateString();
            this.ViewData["totalOrders"] = profile.TotalOrders.ToString();

            return this.FileViewResponse("/profile");
        }

        public IHttpResponse Orders(IHttpRequest request)
        {
            if (!request.Session.Contains(SessionStore.CurrentUserKey))
            {
                throw new InvalidOperationException("There is no logged in user.");
            }

            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);
            
            var orders = this.users.Orders(username);

            var result = orders.Select(o => $@"<tr><td style=""border: solid black 1px ""><a href=""/orderDetails/{o.Id}"">{o.Id}</a></td><td style=""border: solid black 1px "">{o.CreatedData.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)}</td><td style=""border: solid black 1px "">${o.Sum:f2}</td></tr>");

            this.ViewData["content"] = string.Join(Environment.NewLine, result);

            return this.FileViewResponse("/orders");
        }

        public IHttpResponse Order(IHttpRequest request, int id)
        {
            var products = this.users.OrderProducts(id);

            if (!request.Session.Contains(SessionStore.CurrentUserKey))
            {
                throw new InvalidOperationException("There is order with this Id!");
            }

            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);

            var order = this.users.Orders(username).FirstOrDefault(o => o.Id == id);

            if (products == null)
            {
                throw new InvalidOperationException("There is order with this Id!");
            }

            this.ViewData["orderId"] = id.ToString();

            var result =
                products.Select(p => $@"<tr><td style=""border: solid black 1px""><a href=""/cakes/{id}"">{p.Name}</a></td><td style=""border: solid black 1px"">${p.Price:f2}</td></tr>");

            this.ViewData["content"] = string.Join(Environment.NewLine, result);
            this.ViewData["created"] = order.CreatedData.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

            return this.FileViewResponse("/order");
        }

        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            return new RedirectResponse("/login");
        }

        private void SetDefaultViewData()
        {

            this.ViewData["authDisplay"] = "none";
        }

        private void LoginUser(IHttpRequest request, string username)
        {
            request.Session.Add(SessionStore.CurrentUserKey, username);
            request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());

        }

    }
}