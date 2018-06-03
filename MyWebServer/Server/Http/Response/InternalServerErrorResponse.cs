using System;
using MyWebServer.Server.Common;
using MyWebServer.Server.Enums;

namespace MyWebServer.Server.Http.Response
{
    public class InternalServerErrorResponse : ViewResponse
    {
        public InternalServerErrorResponse(Exception ex)
            : base(HttpStatusCode.InternalServerError, new InternalServerErrorView(ex))
        {
            
        }
    }
}