using ConsoleApp;
using HttpServer.Framework.HttpResponses;
using MiniHttpServer.Framework.Authentication;
using System.Net;

namespace HttpServer.Controllers
{
    internal class ControllerBase
    {
        protected HttpListenerContext Context { get; private set; }

        protected IUserSessionService SessionService
        {
            get;
            private set;
        } = null!;

        /// <summary>
        /// Инициализирует контроллер для текущего запроса.
        /// </summary>
        /// <param name="context">
        /// Контекст HTTP-запроса.
        /// </param>
        /// <param name="sessionService">
        /// Сервис пользовательских сеансов.
        /// </param>
        public void Initialize(
            HttpListenerContext context,
            IUserSessionService sessionService)
        {
            Context = context;
            SessionService = sessionService;
        }

        protected JsonResult Json(object obj) => new JsonResult(obj);
        protected HtmlResult Html(string html) => new HtmlResult(html);


        protected HtmlResult Html(string templateFileName, object model)
        {
            var render = new HtmlTemplateRenderer();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "static", templateFileName);

            string renderHtml = render.RenderFromFile(path, model);
            return Html(renderHtml);
        }
    }
}
