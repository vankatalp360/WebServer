using System.Collections.Generic;
using MyWebServer.ByTheCake.Infrastructure;
using MyWebServer.ByTheCake.Models;
using MyWebServer.Server.Http;
using MyWebServer.Server.Http.Contracts;
using MyWebServer.Server.Http.Response;

namespace MyWebServer.ByTheCake.Controllers
{
    public class AccountController : Controller
    {
        public IHttpResponse Login()
        {
            this.ViewData["showError"] = "none";

            this.ViewData["authDisplay"] = "none";

            return this.FileViewResponse(@"\login");
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            const string formNameKey = "name";
            const string formPasswordKey = "password";

            if (!request.FormData.ContainsKey(formNameKey)
                || !request.FormData.ContainsKey(formPasswordKey))
            {
                return new RedirectResponse("/login");
            }

            var name = request.FormData[formNameKey];
            var password = request.FormData[formPasswordKey];

            if (string.IsNullOrWhiteSpace(name)
                || string.IsNullOrWhiteSpace(password))
            {
                this.ViewData["error"] = "You have empty fields";
                this.ViewData["showError"] = "block";

                return this.FileViewResponse(@"/login");
            }

            request.Session.Add(SessionStore.CurrentUserKey, name);
            request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());

            var response = new RedirectResponse("/");
            return response;
        }

        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            return new RedirectResponse("/login");
        }
    }
}