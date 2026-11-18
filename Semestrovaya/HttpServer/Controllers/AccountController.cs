using HttpServer.Controllers;
using HttpServer.Framework.Attributes;
using HttpServer.Framework.HttpResponses;
using MiniHttpServer.Models;
using MiniHttpServer.ORM;
using System.Net;

namespace MiniHttpServer.Controllers
{
    [HttpController("adminLogin")]
    internal class AccountController : ControllerBase
    {
        private static string connectionString = "Host=localhost;Port=5432;Database=myDataBase;Username=postgres;Password=piskoed228";
        private readonly IOrmContext ormContext = new OrmContext(connectionString);

        [Get("")]
        public IHttpResponseTypeResult LoginPage()
        {
            return Html("semestrovaya/Login.html", new {});
        }

        [Post("login")]
        public IHttpResponseTypeResult Login(string login, string password)
        {
            var res = ormContext.GetAllAsync<User>("users").GetAwaiter().GetResult();
            var user = res.FirstOrDefault(r => r.Login == login && r.Password == password);

            if (user == null)
            {
                return new JsonResult(new { Success = false, Message = "Неверный логин или пароль" }, 401);
            }

            int userId = user.Id;

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