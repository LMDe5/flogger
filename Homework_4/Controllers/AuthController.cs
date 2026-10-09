using HttpServer.Framework.Attributes;

namespace HttpServer.Controllers
{
    [HttpController("auth")]
    internal class AuthController
    {
        [Get("login")]
        public void login()
        {
            //TODO: возвращать login.html
            Console.WriteLine("Get login вызван");
        }

        [Post("login")]
        public void login(string login, string password)
        {
            //TODO: Дома делать вывод в консоль получения login/password из form(и query)
            Console.WriteLine($"Post:\nlogin: {login}\npassword: {password}");
        }
    }
}