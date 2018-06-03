using System;
using MyWebServer.Server.Contracts;

namespace MyWebServer.Server.Common
{
    public class InternalServerErrorView : IView
    {
        private readonly Exception execption;

        public InternalServerErrorView(Exception execption)
        {
            this.execption = execption;
        }
        public string View()
        {
            return $"<h1>{this.execption.Message}</h1><h2>{this.execption.StackTrace}</h2>";
        }
    }
}