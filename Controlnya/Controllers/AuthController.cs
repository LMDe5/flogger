using controlnyaMeow.Core.Attributes;
using System.Text.Json;

namespace controlnyaMeow.Controllers
{
    [HttpController("auth")]
    internal class AuthController : ControllerBase
    {
        [Get("login")]
        public void login()
        {
            Console.WriteLine("Get login вызван");
        }

        [Post("login")]
        public void login(string login, string password)
        {
            Console.WriteLine($"Привет из аутчКонтроллер!");
            //TODO: Дома делать вывод в консоль получения login/password из form(и query)
            Console.WriteLine($"Post:\nlogin: {login}\npassword: {password}");
        }

    }
}