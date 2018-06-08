using MyWebServer.Server.Http.Contracts;

namespace MyWebServer.GameStore.Controllers
{
    public class HomeController : GameStoreController
    {
        public IHttpResponse Home(IHttpRequest request)
        {
            var isAdmin = this.SetIdentity(request);

            return this.FileViewResponse("/home");
        }
    }
}