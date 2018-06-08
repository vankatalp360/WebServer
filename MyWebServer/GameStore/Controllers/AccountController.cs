using MyWebServer.ByTheCake.ViewModels;
using MyWebServer.GameStore.Services;
using MyWebServer.GameStore.ViewModels;
using MyWebServer.Infrastructure;
using MyWebServer.Server.Http;
using MyWebServer.Server.Http.Contracts;
using MyWebServer.Server.Http.Response;

namespace MyWebServer.GameStore.Controllers
{
    public class AccountController : GameStoreController
    {
        private const string RegisterView = "account/register";
        private const string LoginView = "account/login";
        private UserService users;

        public AccountController()
        {
            this.users = new UserService();
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            this.SetIdentity(request);

            return this.FileViewResponse(RegisterView);
        }
        public IHttpResponse Register(RegisterViewModel model)
        {
            this.DefaultIdentity();
            var error = this.ValidateModel(model);
            if (error != null)
            {
                this.AddError(error);
                return this.FileViewResponse(RegisterView);
            }

            var success = this.users.Create(model.Email, model.Name, model.Password);

            if (!success)
            {
                error = $"This email is already taken!";
                this.AddError(error);
                return this.FileViewResponse(RegisterView);
            }

            return new RedirectResponse(LoginView);
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            this.SetIdentity(request);

            return this.FileViewResponse("account/login");
        }
        public IHttpResponse Login(IHttpRequest request, LoginViewModel model)
        {
            this.DefaultIdentity();
            var email = model.Email;
            var password = model.Password;

            var success = this.users.FindUser(email, password);

            if (!success)
            {
                this.AddError("Incorect <strong>Email</strong> or <strong>Password</strong>");
                return this.FileViewResponse(LoginView);
            }

            this.LoginUser(request, email);

            return new RedirectResponse("/home");
        }
        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            return new RedirectResponse("/");
        }
        

        private void LoginUser(IHttpRequest request, string email)
        {
            request.Session.Add(SessionStore.CurrentUserKey, email);
            request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }


        
    }
}