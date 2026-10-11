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
        public void Login()
        {
            Console.WriteLine("Get login вызван");
        }

        [Post("login")]
        public void Login(string login, string password)
        {
            //TODO: Дома делать вывод в консоль получения login/password из form(и query)
            Console.WriteLine($"Post:\nlogin: {login}\npassword: {password}");
        }

        //[Get("html")]
        //public JsonResult Html()
        //{
        //    string html = $"<>";

        //    return html //перевести в байты
        //}


        //[Get("json")]
        //public JsonResult Json()
        //{
        //    var json = new { Username = "user", Password="pisun", Age = 13 };

        //    var result = JsonSerializer.Serialize(json);
        //    return result //перевести в байты
        //}

        //[Get("json")]
        //public byte[] Json2()
        //{
        //    var json = new { Username = "user", Age = 13 };

        //    var result = JsonSerializer.Serialize(json);
        //    return result //перевести в байты
        //}
    }
}