using HttpServer.Framework;
using HttpServer.Framework.Attributes;
using HttpServer.Framework.HttpResponses;
using System.Text.Json;

namespace HttpServer.Controllers
{
    [HttpController("auth")]
    internal class AuthController : ControllerBase
    {
        [Get("login")]
        public void login()
        {
           var session = Context.Request.Headers;

            Console.WriteLine("Get login вызван");
        }

        [Post("login")]
        public void login(string login, string password)
        {
            Console.WriteLine($"Привет из аутчКонтроллер!");
            //TODO: Дома делать вывод в консоль получения login/password из form(и query)
            Console.WriteLine($"Post:\nlogin: {login}\npassword: {password}");
        }

        [Get("html")]
        public IHttpResponseTypeResult Html()
        {
            string html = "<h2>Привет из контроллера</h2>";
            return Html(html); //привести к байтам
        }

        [Get("json")]
        public IHttpResponseTypeResult Json()
        {
            var json = new {Username = "user", Password = "1234", age = 13};
            var result = JsonSerializer.Serialize(json);
            return Json(json);
        }

        [Get("json2")]
        public IHttpResponseTypeResult Json2()
        {
            var json = new { Username = "user", Password = "1234", age = 13 };
            var result = JsonSerializer.Serialize(json);
            return Json(json);
        }
    }
}