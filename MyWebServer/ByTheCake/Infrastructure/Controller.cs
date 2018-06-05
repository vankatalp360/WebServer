using System.Collections.Concurrent;
using MyWebServer.Server.Enums;
using MyWebServer.Server.Http.Contracts;
using MyWebServer.Server.Http.Response;

namespace MyWebServer.ByTheCake.Infrastructure
{
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Views.Home;

    public abstract class Controller 
    {
        public const string DefaultPath = @"ByTheCake\Resources\{0}.html";
        public const string ContentPlaceholder = "{{{content}}}";

        protected Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                ["authDisplay"] = "block"
            };
            
            this.ViewData["showError"] = "none";
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

        protected string ProcessFileHtml(string fileName)
        {
            var layoutHtml = File.ReadAllText(string.Format(DefaultPath, "layout"));

            var fileHtml = File
                .ReadAllText(string.Format(DefaultPath, fileName));

            var result = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            return result;
        }
    }
}
