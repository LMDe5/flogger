using HttpServer.Framework;
using HttpServer.Framework.Attributes;

namespace HttpServer.Controllers
{
    [HttpController("steam")]
    internal class SteamController : ControllerBase
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
            Console.WriteLine($"Привет из стимКонтроллер!");
            //TODO: Дома делать вывод в консоль получения login/password из form(и query)
            Console.WriteLine($"Post:\nlogin: {login}\npassword: {password}");
        }
    }
}