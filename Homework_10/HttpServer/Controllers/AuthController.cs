using HttpServer.Framework.Attributes;
using HttpServer.Framework.HttpResponses;
using MiniHttpServer.Services.Authentication;
using System.Net;

namespace HttpServer.Controllers
{
    [HttpController("auth")]
    internal class AuthController : ControllerBase
    {
        [Get("test")]
        public IHttpResponseTypeResult Test()
        {
            return Html("OK");
        }

        [Get("")]
        public void login()
        {
           var session = Context.Request.Headers;

            Console.WriteLine("Get login вызван");
        }

        [Get("profile")]
        [Authorize]
        public IHttpResponseTypeResult Profile()
        {
            return Html("<h1>Личный кабинет</h1>");
        }

        [Post("login")]
        public IHttpResponseTypeResult Login(string login, string password)
        {
            const string testLogin = "student@g.com";
            const string testPassword = "test123";

            if (login != testLogin || password != testPassword)
            {
                return new JsonResult(new { Success = false, Message = "Неверный логин или пароль" }, 401);
            }

            int userId = 1;

            string sessionId = SessionService.CreateSession(userId);

            var cookie = new Cookie("sessionId", sessionId, "/")
            {
                HttpOnly = true,
                Secure = Context.Request.IsSecureConnection,
                Expires = DateTime.UtcNow.AddMinutes(30)
            };

            Context.Response.SetCookie(cookie);

            return Json(new
            {
                Success = true,
                Message = "Вход выполнен"
            });
        }

        [Get("logOut")]
        public IHttpResponseTypeResult Logout()
        {
            string? sessionId = Context.Request.Cookies["sessionId"]?.Value;

            SessionService.RemoveSession(sessionId);

            var cookie = new Cookie("sessionId", string.Empty, "/")
            {
                HttpOnly = true,
                Secure = Context.Request.IsSecureConnection,
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            Context.Response.SetCookie(cookie);

            return Json(new
            {
                Success = true,
                Message = "Выход выполнен"
            });
        }
    }
}