using System;
using System.ComponentModel.DataAnnotations;
using MyWebServer.GameStore.Services;
using MyWebServer.GameStore.Services.Contracts;
using MyWebServer.Infrastructure;
using MyWebServer.Server.Http;

namespace MyWebServer.GameStore
{
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public abstract class GameStoreController
    {
        public string DefaultPath = @"\Resources\{0}.html";
        public const string ContentPlaceholder = "{{{content}}}";
        private IUserService users;

        protected GameStoreController()
        {
            users = new UserService();

            this.ViewData = new Dictionary<string, string>
            {
                ["authDisplay"] = "block"
            };

            this.ViewData["showError"] = "none";

            this.TakeDefaultPathMEthod();
        }

        private void TakeDefaultPathMEthod()
        {
            var type = this.GetType();
            var fullName = type.FullName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var app = fullName[1];

            DefaultPath = $"{app}{DefaultPath}";
        }

        protected IDictionary<string, string> ViewData { get; private set; }



        protected IHttpResponse FileViewResponse(string fileName)
        {
            var result = this.ProcessFileHtml(fileName);

            if (this.ViewData.Any())
            {
                foreach (var value in this.ViewData)
                {
                    result = result.Replace($"{{{{{{{value.Key}}}}}}}", value.Value);
                }
            }

            return new ViewResponse(HttpStatusCode.Ok, new FileView(result));
        }

        protected void AddError(string errorMessage)
        {
            this.ViewData["error"] = errorMessage;
            this.ViewData["showError"] = "block";
        }

        protected string ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(model, context, results, true) == false)
            {
                foreach (var result in results)
                {
                    if (result != ValidationResult.Success)
                    {
                        return result.ErrorMessage;
                    }
                }
            }

            return null;
        }

        protected string ProcessFileHtml(string fileName)
        {
            var layoutHtml = File.ReadAllText(string.Format(DefaultPath, "layout"));

            var fileHtml = File
                .ReadAllText(string.Format(DefaultPath, fileName));

            var result = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            return result;
        }

        protected bool SetIdentity(IHttpRequest request)
        {
            var contains = request.Session.Contains(SessionStore.CurrentUserKey);
            
            if (!contains)
            {
                this.ViewData["guestDisplay"] = "block";
                this.ViewData["userDisplay"] = "none !important";
                this.ViewData["adminDisplay"] = "none !important";
                return false;
            }

            var email = request.Session.Get(SessionStore.CurrentUserKey).ToString();
            var isAdmin = users.IsAdmin(email);

            if (isAdmin)
            {
                this.ViewData["guestDisplay"] = "none !important";
                this.ViewData["userDisplay"] = "none !important";
                this.ViewData["adminDisplay"] = "block";
                return true;
            }
            else
            {
                this.ViewData["guestDisplay"] = "none !important";
                this.ViewData["userDisplay"] = "block";
                this.ViewData["adminDisplay"] = "none !important";
                return false;
            }

        }

        protected void DefaultIdentity()
        {
            this.ViewData["guestDisplay"] = "block";
            this.ViewData["userDisplay"] = "none !important";
            this.ViewData["adminDisplay"] = "none !important";
        }
    }
}