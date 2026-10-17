using HttpServer.Framework;
using HttpServer.Framework.Attributes;

namespace HttpServer.Controllers
{
    [HttpController("home-work")]
    internal class HomeworkController : ControllerBase
    {
        [Get("send")]
        public void Send(string to)
        {
            string zipPath = Path.Combine(Directory.GetCurrentDirectory(), "static", "Homework_5.zip");

            if (!File.Exists(zipPath))
            {
                Console.WriteLine("Делил, добавь файл, мряу");
                return;
            }

            MessageSender sender = new MessageSender();
            sender.SendMessage(to, zipPath);
        }
    }
}
