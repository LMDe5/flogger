using HttpServer.Controllers;
using HttpServer.Framework.Attributes;
using HttpServer.Framework.HttpResponses;
using ServiceStack.Web;
using System.Net;
using System.Text.Json;

namespace MiniHttpServer.Controllers
{
    [HttpController("cookie")]
    internal class CookieController : ControllerBase
    {
        [Get("set")]
        public IHttpResponseTypeResult Set()
        {
            var cookie = new Cookie(
                "testCookie",
                "HelloWorld",
                "/")
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(30)
            };

            Context.Response.SetCookie(cookie);

            return Html("<h1>Cookie установлена</h1>");
        }

        [Get("get")]
        public IHttpResponseTypeResult Get()
        {
            string? value =
                Context.Request.Cookies["testCookie"]?.Value;

            return Json(new
            {
                Value = value
            });
        }

        [Get("delete")]
        public IHttpResponseTypeResult Delete()
        {
            var cookie = new Cookie(
                "testCookie",
                string.Empty,
                "/")
            {
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            Context.Response.SetCookie(cookie);

            return Html("<h1>Cookie удалена</h1>");
        }
    }
}
