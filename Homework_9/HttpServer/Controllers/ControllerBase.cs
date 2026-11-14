using ConsoleApp;
using HttpServer.Framework.HttpResponses;
using System.Net;

namespace HttpServer.Controllers
{
    internal class ControllerBase
    {
        private HtmlTemplateRenderer render = new HtmlTemplateRenderer();
        protected HttpListenerContext Context { get; private set; }

        internal void SetContext(HttpListenerContext Context)
        {
            this.Context = Context;
        } 

        protected JsonResult Json(object obj) => new JsonResult(obj);
        protected HtmlResult Html(string html) => new HtmlResult(html);


        protected HtmlResult Html(string templateFileName, object model)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "static", templateFileName);

            string renderHtml = render.RenderFromFile(path, model);
            return Html(renderHtml);
        }
    }
}
