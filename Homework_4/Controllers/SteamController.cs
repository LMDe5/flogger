using HttpServer.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer.Controllers
{
    [HttpController("steam")]
    internal class SteamController
    {
        [Get("login")]
        public void login()
        {
            //TODO: возвращать login.html
            Console.WriteLine("Get login Steam вызван");
        }

        [Post("login")]
        public void login(string login, string password)
        {
            //TODO: Дома делать вывод в консоль получения login/password из form(и query)
            Console.WriteLine($"SteamPost:\nlogin: {login}\npassword: {password}");
        }
    }
}
