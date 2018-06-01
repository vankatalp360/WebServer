using MyWebServer.Server.Http;

namespace MyWebServer.Application
{
    using Application.Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class MainApplication : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.Get(
                "/", 
                req => new HomeController().Index());

            appRouteConfig.Get(
                "/testsession",
                req => new HomeController().SessionTest(req));
            
            appRouteConfig.Post(
                "register",
                httpContext => new UserController()
                .RegisterPost(httpContext.FormData["name"]));

            appRouteConfig.Get(
                "register",
                httpContext => new UserController()
                .RegisterGet());
            
            appRouteConfig.Get(
                "/user/{(?<name>[a-zA-Z0-9]+)}",
                httpContext => new UserController()
                .Details(httpContext.UrlParameters["name"]));
        }
    }
}
