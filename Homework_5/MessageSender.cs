using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace HttpServer
{
    public class MessageSender
    {
        private string host = "smtp.yandex.ru";
        private int port = 465;

        private string fromName = "Amir Mukhammadiev";
        private string fromLogin = "Forexample1488@yandex.ru";
        private string fromPassword = "augjesbalixclyng";

        public void SendMessage(string toAddres, string zipPath)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(fromName, fromLogin));
            message.To.Add(new MailboxAddress("", toAddres));

            var builder = new BodyBuilder();
            builder.TextBody = "группа 11-541, Мухаммадиев Амир";
            builder.Attachments.Add(zipPath);
            message.Body = builder.ToMessageBody();

            using (var client  = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                client.Connect(host, port, SecureSocketOptions.SslOnConnect);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(fromLogin, fromPassword);

                client.Send(message);
                client.Disconnect(true);
            }

            Console.WriteLine($"Письмо отправлено на почту {toAddres}");
        }
    }
}
