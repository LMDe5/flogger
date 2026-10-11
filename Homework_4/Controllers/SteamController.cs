using HttpServer.Framework;
using HttpServer.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using HtmlAgilityPack;

namespace HttpServer.Controllers
{
    [HttpController("steam")]
    internal class SteamController : ControllerBase
    {
        //http://localhost:2323/steam/send?to=test&test.ru
        [Get("send")]
        public void Send(string toStr, string hero)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("somemail@gmail.com", "Tom");
            // кому отправляем
            MailAddress to = new MailAddress($"{toStr}");
            // создаем объект сообщения
            MailMessage message = new MailMessage(from, to);
            // тема письма
            message.Subject = "Тест";
            // текст письма
            message.Body = "<h2>Письмо-тест работы smtp-клиента</h2>";
            // письмо представляет код html
            message.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("Forexample1488", "Forexample6769");
            smtp.EnableSsl = true;
            smtp.Send(message);
            Console.Read();
        }

        private void Load(string path)
        {
            var document = new HtmlDocument();
            document.LoadHtml(path);

            var nodes = document.DocumentNode.SelectNodes("//div[@class='info']");

            foreach (var node in nodes)
            {
                string infoContent = node.InnerText.Trim();
                // Данные infoContent теперь в вашем распоряжении.
            }
        }
    }
}
